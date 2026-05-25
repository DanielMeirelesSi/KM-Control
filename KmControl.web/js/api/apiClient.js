const API_BASE_URL = "http://localhost:5250/api";

export async function apiRequest(endpoint, options = {}) {
    const response = await fetch(`${API_BASE_URL}${endpoint}`, {
        headers: {
            "Content-Type": "application/json",
            ...options.headers
        },
        ...options
    });

    if (!response.ok) {
        const errorMessage = await response.text();
        throw new Error(errorMessage || "Erro ao comunicar com a API.");
    }

    if (response.status === 204) {
        return null;
    }

    return response.json();
}