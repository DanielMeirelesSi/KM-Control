export function formatarMoeda(valor) {
    return Number(valor || 0).toLocaleString("pt-BR", {
        style: "currency",
        currency: "BRL"
    });
}

export function formatarKm(valor) {
    return `${Number(valor || 0).toLocaleString("pt-BR", {
        minimumFractionDigits: 0,
        maximumFractionDigits: 2
    })} km`;
}

export function formatarLitros(valor) {
    return `${Number(valor || 0).toLocaleString("pt-BR", {
        minimumFractionDigits: 0,
        maximumFractionDigits: 2
    })} L`;
}

export function formatarKmPorLitro(valor) {
    return `${Number(valor || 0).toLocaleString("pt-BR", {
        minimumFractionDigits: 0,
        maximumFractionDigits: 2
    })} km/l`;
}

export function formatarData(data) {
    if (!data) {
        return "-";
    }

    const dataFormatada = new Date(data);

    return dataFormatada.toLocaleDateString("pt-BR");
}