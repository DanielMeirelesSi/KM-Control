import { apiRequest } from "../api/apiClient.js";

export async function listarVeiculos() {
    return apiRequest("/veiculos");
}

export async function buscarVeiculoPorId(id) {
    return apiRequest(`/veiculos/${id}`);
}

export async function cadastrarVeiculo(dadosVeiculo) {
    return apiRequest("/veiculos", {
        method: "POST",
        body: JSON.stringify(dadosVeiculo)
    });
}

export async function atualizarVeiculo(id, dadosVeiculo) {
    return apiRequest(`/veiculos/${id}`, {
        method: "PUT",
        body: JSON.stringify(dadosVeiculo)
    });
}

export async function excluirVeiculo(id) {
    return apiRequest(`/veiculos/${id}`, {
        method: "DELETE"
    });
}