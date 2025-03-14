import Factura from "../types/Factura";

const fetchFacturas = async (): Promise<Factura[]> => {
    try {
      const response = await fetch("https://localhost:7049/api/Facturas");

      if (!response.ok) {
        throw new Error("Error al obtener las facturas");
      }

      const data = await response.json();
      return data.data;
    } catch (error) {
      console.error(error);
      return [];
    }
  };

  const fetchFacturaById = async (id: string): Promise<Factura | null> => {
    try {
      const response = await fetch(`https://localhost:7049/api/Facturas/${id}`);
      if (!response.ok) {
        throw new Error("Error al obtener la factura");
      }
      const data = await response.json();
      return data.data;
    } catch (error) {
      console.error(error);
      return null;
    }
  };

  export const enviarRecordatorio = async (facturaId: string) => {
    const response = await fetch(`https://localhost:7049/api/Facturas/enivar-email/${facturaId}`, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
    });
  
    if (!response.ok) {
       if(response.status === 404) {
        throw new Error("No se encontro la factura");
      }
      if(response.status === 410) {
        throw new Error("La factura ya fue procesada");
      }
    };
    const data = await response.json();
    return data;
  }

  export {
    fetchFacturas,
    fetchFacturaById,
  }