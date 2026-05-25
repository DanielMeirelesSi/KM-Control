import { apiRequest } from "../api/apiClient.js";

export async function listarAbastecimentos() {
    return apiRequest("/abastecimentos");
}

export async function listarAbastecimentosPorVeiculo(veiculoId) {
    return apiRequest(`/abastecimentos/veiculo/${veiculoId}`);
}

export async function obterResumoPorVeiculo(veiculoId) {
    return apiRequest(`/abastecimentos/veiculo/${veiculoId}/resumo`);
}

export async function obterResumoGeral() {
    return apiRequest("/abastecimentos/resumo-geral");
}

export async function cadastrarAbastecimento(dadosAbastecimento) {
    return apiRequest("/abastecimentos", {
        method: "POST",
        body: JSON.stringify(dadosAbastecimento)
    });
}

export async function atualizarAbastecimento(id, dadosAbastecimento) {
    return apiRequest(`/abastecimentos/${id}`, {
        method: "PUT",
        body: JSON.stringify(dadosAbastecimento)
    });
}

export async function excluirAbastecimento(id) {
    return apiRequest(`/abastecimentos/${id}`, {
        method: "DELETE"
    });
}