import {
    listarVeiculos,
    cadastrarVeiculo,
    atualizarVeiculo,
    excluirVeiculo
} from "./services/veiculoService.js";

import {
    listarAbastecimentos,
    obterResumoGeral,
    obterResumoPorVeiculo,
    cadastrarAbastecimento,
    excluirAbastecimento
} from "./services/abastecimentoService.js";

import {
    formatarMoeda,
    formatarKm,
    formatarLitros,
    formatarKmPorLitro,
    formatarData
} from "./utils/formatters.js";

const navItems = document.querySelectorAll(".nav-item");
const pageSections = document.querySelectorAll(".page-section");
const pageTitle = document.getElementById("page-title");
const pageSubtitle = document.getElementById("page-subtitle");
const sectionTargetButtons = document.querySelectorAll("[data-section-target]");
let veiculoEmEdicaoId = null;

const sectionInfo = {
    dashboard: {
        title: "Dashboard",
        subtitle: "Visão geral dos seus veículos e abastecimentos"
    },
    veiculos: {
        title: "Veículos",
        subtitle: "Cadastre, edite e acompanhe seus veículos"
    },
    abastecimentos: {
        title: "Abastecimentos",
        subtitle: "Registre abastecimentos e acompanhe seu histórico"
    },
    resumos: {
        title: "Resumos",
        subtitle: "Analise médias, custos e indicadores dos seus veículos"
    }
};

function showSection(sectionName) {
    pageSections.forEach(section => {
        section.classList.remove("active");
    });

    navItems.forEach(item => {
        item.classList.remove("active");
    });

    const selectedSection = document.getElementById(`${sectionName}-section`);
    const selectedNavItem = document.querySelector(`.nav-item[data-section="${sectionName}"]`);

    if (selectedSection) {
        selectedSection.classList.add("active");
    }

    if (selectedNavItem) {
        selectedNavItem.classList.add("active");
    }

    if (sectionInfo[sectionName]) {
        pageTitle.textContent = sectionInfo[sectionName].title;
        pageSubtitle.textContent = sectionInfo[sectionName].subtitle;
    }

    window.scrollTo({
        top: 0,
        behavior: "smooth"
    });
}

navItems.forEach(item => {
    item.addEventListener("click", () => {
        const sectionName = item.dataset.section;
        showSection(sectionName);
    });
});

sectionTargetButtons.forEach(button => {
    button.addEventListener("click", () => {
        const sectionName = button.dataset.sectionTarget;
        showSection(sectionName);
    });
});

function atualizarCardsResumoGeral(resumo) {
    document.getElementById("total-veiculos").textContent = resumo.quantidadeVeiculos ?? 0;
    document.getElementById("total-abastecimentos").textContent = resumo.quantidadeAbastecimentos ?? 0;
    document.getElementById("total-gasto").textContent = formatarMoeda(resumo.totalGasto);
    document.getElementById("media-geral").textContent = formatarKmPorLitro(resumo.mediaGeralKmPorLitro);

    const resumoGeralVeiculos = document.getElementById("resumo-geral-veiculos");
    const resumoGeralAbastecimentos = document.getElementById("resumo-geral-abastecimentos");
    const resumoGeralKm = document.getElementById("resumo-geral-km");
    const resumoGeralGasto = document.getElementById("resumo-geral-gasto");
    const resumoGeralMedia = document.getElementById("resumo-geral-media");
    const resumoGeralCusto = document.getElementById("resumo-geral-custo");

    if (resumoGeralVeiculos) {
        resumoGeralVeiculos.textContent = resumo.quantidadeVeiculos ?? 0;
    }

    if (resumoGeralAbastecimentos) {
        resumoGeralAbastecimentos.textContent = resumo.quantidadeAbastecimentos ?? 0;
    }

    if (resumoGeralKm) {
        resumoGeralKm.textContent = formatarKm(resumo.totalKmRodado);
    }

    if (resumoGeralGasto) {
        resumoGeralGasto.textContent = formatarMoeda(resumo.totalGasto);
    }

    if (resumoGeralMedia) {
        resumoGeralMedia.textContent = formatarKmPorLitro(resumo.mediaGeralKmPorLitro);
    }

    if (resumoGeralCusto) {
        resumoGeralCusto.textContent = formatarMoeda(resumo.custoMedioPorKm);
    }
}

function criarCardVeiculo(veiculo, mostrarAcoes = false) {
    return `
        <article class="vehicle-card">
            <div>
                <h4>${veiculo.nome}</h4>
                <p>${veiculo.modelo} • ${veiculo.ano} • ${veiculo.tipo}</p>
                <p>Placa: ${veiculo.placa || "Não informada"}</p>
            </div>

            <div class="vehicle-card-side">
                <p>Odômetro</p>
                <strong>${formatarKm(veiculo.odometroAtual)}</strong>

                ${
                    mostrarAcoes
                        ? `
                            <div class="card-actions">
                                <button 
                                    class="action-button edit" 
                                    data-action="editar-veiculo" 
                                    data-id="${veiculo.id}"
                                    data-nome="${veiculo.nome}"
                                    data-modelo="${veiculo.modelo}"
                                    data-ano="${veiculo.ano}"
                                    data-tipo="${veiculo.tipo}"
                                    data-placa="${veiculo.placa || ""}"
                                    data-odometro="${veiculo.odometroAtual}"
                                >
                                    Editar
                                </button>

                                <button class="action-button danger" data-action="excluir-veiculo" data-id="${veiculo.id}">
                                    Excluir
                                </button>
                            </div>
                        `
                        : ""
                }
            </div>
        </article>
    `;
}

function renderizarVeiculos(veiculos) {
    const listaVeiculos = document.getElementById("lista-veiculos");
    const listaVeiculosDashboard = document.getElementById("lista-veiculos-dashboard");

    const mensagemVazia = `<p class="empty-message">Nenhum veículo cadastrado ainda.</p>`;

    if (!veiculos.length) {
        if (listaVeiculos) {
            listaVeiculos.innerHTML = mensagemVazia;
        }

        if (listaVeiculosDashboard) {
            listaVeiculosDashboard.innerHTML = mensagemVazia;
        }

        return;
    }

    if (listaVeiculos) {
        listaVeiculos.innerHTML = veiculos
            .map(veiculo => criarCardVeiculo(veiculo, true))
            .join("");
    }

    if (listaVeiculosDashboard) {
        listaVeiculosDashboard.innerHTML = veiculos
            .slice(0, 3)
            .map(veiculo => criarCardVeiculo(veiculo, false))
            .join("");
    }
}

function preencherSelectVeiculos(veiculos) {
    const selects = [
        document.getElementById("abastecimento-veiculo"),
        document.getElementById("resumo-veiculo"),
        document.getElementById("dashboard-resumo-veiculo")
    ];

    selects.forEach(select => {
        if (!select) return;

        select.innerHTML = `<option value="">Selecione o veículo</option>`;

        veiculos.forEach(veiculo => {
            const option = document.createElement("option");
            option.value = veiculo.id;
            option.textContent = `${veiculo.nome} - ${veiculo.modelo}`;
            select.appendChild(option);
        });
    });
}

function criarLinhaAbastecimento(abastecimento, mostrarAcoes = false) {
    return `
        <tr>
            <td>${formatarData(abastecimento.data)}</td>
            <td>${abastecimento.nomeVeiculo}</td>
            <td>${formatarKm(abastecimento.kmRodado)}</td>
            <td>${formatarLitros(abastecimento.litrosAbastecidos)}</td>
            <td>${formatarMoeda(abastecimento.valorTotal)}</td>
            <td>${formatarKmPorLitro(abastecimento.mediaKmPorLitro)}</td>
            ${
                mostrarAcoes
                    ? `
                        <td>
                            <div class="table-actions">
                                <button class="action-button danger" data-action="excluir-abastecimento" data-id="${abastecimento.id}">
                                    Excluir
                                </button>
                            </div>
                        </td>
                    `
                    : ""
            }
        </tr>
    `;
}

function renderizarAbastecimentos(abastecimentos) {
    const tabelaAbastecimentos = document.getElementById("tabela-abastecimentos");
    const tabelaDashboard = document.getElementById("tabela-abastecimentos-dashboard");

    const mensagemVaziaTabelaCompleta = `
        <tr>
            <td colspan="7">Nenhum abastecimento cadastrado ainda.</td>
        </tr>
    `;

    const mensagemVaziaDashboard = `
        <tr>
            <td colspan="6">Nenhum abastecimento cadastrado ainda.</td>
        </tr>
    `;

    if (!abastecimentos.length) {
        if (tabelaAbastecimentos) {
            tabelaAbastecimentos.innerHTML = mensagemVaziaTabelaCompleta;
        }

        if (tabelaDashboard) {
            tabelaDashboard.innerHTML = mensagemVaziaDashboard;
        }

        return;
    }

    if (tabelaAbastecimentos) {
        tabelaAbastecimentos.innerHTML = abastecimentos
            .map(abastecimento => criarLinhaAbastecimento(abastecimento, true))
            .join("");
    }

    if (tabelaDashboard) {
        tabelaDashboard.innerHTML = abastecimentos
            .slice(0, 5)
            .map(abastecimento => criarLinhaAbastecimento(abastecimento, false))
            .join("");
    }
}

function limparResumoPorVeiculo(prefixo = "") {
    const km = document.getElementById(`${prefixo}resumo-km`);
    const gasto = document.getElementById(`${prefixo}resumo-gasto`);
    const media = document.getElementById(`${prefixo}resumo-media`);
    const custo = document.getElementById(`${prefixo}resumo-custo`);

    if (km) km.textContent = "0 km";
    if (gasto) gasto.textContent = "R$ 0,00";
    if (media) media.textContent = "0 km/l";
    if (custo) custo.textContent = "R$ 0,00";
}

function atualizarResumoPorVeiculo(resumo, prefixo = "") {
    const km = document.getElementById(`${prefixo}resumo-km`);
    const gasto = document.getElementById(`${prefixo}resumo-gasto`);
    const media = document.getElementById(`${prefixo}resumo-media`);
    const custo = document.getElementById(`${prefixo}resumo-custo`);

    if (km) km.textContent = formatarKm(resumo.totalKmRodado);
    if (gasto) gasto.textContent = formatarMoeda(resumo.totalGasto);
    if (media) media.textContent = formatarKmPorLitro(resumo.mediaGeralKmPorLitro);
    if (custo) custo.textContent = formatarMoeda(resumo.custoMedioPorKm);
}

async function configurarSelectsResumo() {
    const resumoVeiculoSelect = document.getElementById("resumo-veiculo");
    const dashboardResumoSelect = document.getElementById("dashboard-resumo-veiculo");

    if (resumoVeiculoSelect) {
        resumoVeiculoSelect.addEventListener("change", async () => {
            const veiculoId = resumoVeiculoSelect.value;

            if (!veiculoId) {
                limparResumoPorVeiculo("");
                return;
            }

            const resumo = await obterResumoPorVeiculo(veiculoId);
            atualizarResumoPorVeiculo(resumo, "");
        });
    }

    if (dashboardResumoSelect) {
        dashboardResumoSelect.addEventListener("change", async () => {
            const veiculoId = dashboardResumoSelect.value;

            if (!veiculoId) {
                limparResumoPorVeiculo("dashboard-");
                return;
            }

            const resumo = await obterResumoPorVeiculo(veiculoId);
            atualizarResumoPorVeiculo(resumo, "dashboard-");
        });
    }
}
function obterDadosFormularioVeiculo() {
    return {
        nome: document.getElementById("veiculo-nome").value.trim(),
        modelo: document.getElementById("veiculo-modelo").value.trim(),
        ano: Number(document.getElementById("veiculo-ano").value),
        tipo: document.getElementById("veiculo-tipo").value,
        placa: document.getElementById("veiculo-placa").value.trim() || null,
        odometroAtual: Number(document.getElementById("veiculo-odometro").value)
    };
}

function limparFormularioVeiculo() {
    document.getElementById("form-veiculo").reset();
}

async function configurarFormularioVeiculo() {
    const formVeiculo = document.getElementById("form-veiculo");

    if (!formVeiculo) {
        return;
    }

    formVeiculo.addEventListener("submit", async (event) => {
        event.preventDefault();

        try {
            const dadosVeiculo = obterDadosFormularioVeiculo();

            if (veiculoEmEdicaoId) {
                await atualizarVeiculo(veiculoEmEdicaoId, dadosVeiculo);
                alert("Veículo atualizado com sucesso!");
            } else {
                await cadastrarVeiculo(dadosVeiculo);
                alert("Veículo cadastrado com sucesso!");
            }

            limparFormularioVeiculo();
            veiculoEmEdicaoId = null;

            const botaoSubmit = formVeiculo.querySelector("button[type='submit']");
            botaoSubmit.textContent = "Salvar veículo";

            await carregarDadosIniciais();
        } catch (error) {
            console.error("Erro ao salvar veículo:", error);
            alert(error.message || "Não foi possível salvar o veículo.");
        }
    });
}

function obterDadosFormularioAbastecimento() {
    return {
        veiculoId: Number(document.getElementById("abastecimento-veiculo").value),
        kmRodado: Number(document.getElementById("km-rodado").value),
        litrosAbastecidos: Number(document.getElementById("litros-abastecidos").value),
        valorTotal: Number(document.getElementById("valor-total").value),
        combustivel: document.getElementById("combustivel").value
    };
}

function limparFormularioAbastecimento() {
    document.getElementById("form-abastecimento").reset();
}

async function configurarFormularioAbastecimento() {
    const formAbastecimento = document.getElementById("form-abastecimento");

    if (!formAbastecimento) {
        return;
    }

    formAbastecimento.addEventListener("submit", async (event) => {
        event.preventDefault();

        try {
            const dadosAbastecimento = obterDadosFormularioAbastecimento();

            await cadastrarAbastecimento(dadosAbastecimento);

            limparFormularioAbastecimento();

            await carregarDadosIniciais();

            alert("Abastecimento cadastrado com sucesso!");
        } catch (error) {
            console.error("Erro ao cadastrar abastecimento:", error);
            alert(error.message || "Não foi possível cadastrar o abastecimento.");
        }
    });
}

function configurarExclusaoAbastecimento() {
    const tabelaAbastecimentos = document.getElementById("tabela-abastecimentos");

    if (!tabelaAbastecimentos) {
        return;
    }

    tabelaAbastecimentos.addEventListener("click", async (event) => {
        const botao = event.target.closest("[data-action='excluir-abastecimento']");

        if (!botao) {
            return;
        }

        const abastecimentoId = Number(botao.dataset.id);

        const confirmarExclusao = confirm("Tem certeza que deseja excluir este abastecimento?");

        if (!confirmarExclusao) {
            return;
        }

        try {
            await excluirAbastecimento(abastecimentoId);

            await carregarDadosIniciais();

            alert("Abastecimento excluído com sucesso!");
        } catch (error) {
            console.error("Erro ao excluir abastecimento:", error);
            alert(error.message || "Não foi possível excluir o abastecimento.");
        }
    });
}

function preencherFormularioEdicaoVeiculo(botao) {
    veiculoEmEdicaoId = Number(botao.dataset.id);

    document.getElementById("veiculo-nome").value = botao.dataset.nome;
    document.getElementById("veiculo-modelo").value = botao.dataset.modelo;
    document.getElementById("veiculo-ano").value = botao.dataset.ano;
    document.getElementById("veiculo-tipo").value = botao.dataset.tipo;
    document.getElementById("veiculo-placa").value = botao.dataset.placa || "";
    document.getElementById("veiculo-odometro").value = botao.dataset.odometro;

    const formVeiculo = document.getElementById("form-veiculo");
    const botaoSubmit = formVeiculo.querySelector("button[type='submit']");

    botaoSubmit.textContent = "Atualizar veículo";

    showSection("veiculos");

    setTimeout(() => {
        formVeiculo.scrollIntoView({
            behavior: "smooth",
            block: "center"
        });
    }, 200);
}

function configurarAcoesVeiculo() {
    const listaVeiculos = document.getElementById("lista-veiculos");

    if (!listaVeiculos) {
        return;
    }

    listaVeiculos.addEventListener("click", async (event) => {
        const botaoEditar = event.target.closest("[data-action='editar-veiculo']");

        if (botaoEditar) {
            preencherFormularioEdicaoVeiculo(botaoEditar);
            return;
        }

        const botaoExcluir = event.target.closest("[data-action='excluir-veiculo']");

        if (!botaoExcluir) {
            return;
        }

        const veiculoId = Number(botaoExcluir.dataset.id);

        const confirmarExclusao = confirm("Tem certeza que deseja excluir este veículo?");

        if (!confirmarExclusao) {
            return;
        }

        try {
            await excluirVeiculo(veiculoId);

            await carregarDadosIniciais();

            alert("Veículo excluído com sucesso!");
        } catch (error) {
            console.error("Erro ao excluir veículo:", error);

            alert(
                error.message ||
                "Não foi possível excluir o veículo. Verifique se ele possui abastecimentos cadastrados."
            );
        }
    });
}

async function carregarDadosIniciais() {
    try {
        const [veiculos, abastecimentos, resumoGeral] = await Promise.all([
            listarVeiculos(),
            listarAbastecimentos(),
            obterResumoGeral()
        ]);

        atualizarCardsResumoGeral(resumoGeral);
        renderizarVeiculos(veiculos);
        preencherSelectVeiculos(veiculos);
        renderizarAbastecimentos(abastecimentos);
    } catch (error) {
        console.error("Erro ao carregar dados iniciais:", error);
        alert("Não foi possível carregar os dados da API. Verifique se a API está rodando.");
    }
}

configurarSelectsResumo();
configurarFormularioVeiculo();
configurarFormularioAbastecimento();
configurarExclusaoAbastecimento();
configurarAcoesVeiculo();
carregarDadosIniciais();