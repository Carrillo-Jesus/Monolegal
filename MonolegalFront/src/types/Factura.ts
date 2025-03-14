import Cliente from "./Cliente";

export default interface Factura {
    id: string;
    codigoFactura: string;
    cliente: Cliente;
    ciudad: string;
    nit: string;
    totalFactura: number;
    subTotal: number;
    iva: number;
    retencion: number;
    fechaCreacion: string;
    estado: number;
    pagada: boolean;
    fechaPago?: string | null;
}