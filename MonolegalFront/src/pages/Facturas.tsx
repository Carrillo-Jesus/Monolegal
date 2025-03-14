import { useEffect, useState } from "react";
import { fetchFacturas, enviarRecordatorio } from "../services/facturasService";
import Factura from "../types/Factura";
import { Link } from "react-router-dom";


const FacturasList = () => {
  const [facturas, setFacturas] = useState<Factura[]>([]);
  const [error, setError] = useState<string | null>(null);
  const [loadingId, setLoadingId] = useState<string | null>(null);
  const [mensaje, setMensaje] = useState<{ id: string; text: string } | null>(null);
 
  useEffect(() => {
    fetchFacturas()
      .then(setFacturas)
      .catch(() => setError("No se pudieron cargar las facturas."));
  }, []);

  const handleEnviarRecordatorio = async (factura: Factura) => {
    setLoadingId(factura.id); 
    setMensaje(null);

    try {
      const respuesta = await enviarRecordatorio(factura.id);
      setMensaje({ id: factura.id, text: respuesta.message });
    } catch (err: unknown) {
      if (err instanceof Error) {
        setMensaje({ id: factura.id, text: err?.message ?? "Error al enviar recordatorio" });
      } else {
        console.error("Ha ocurrido un error");
      }

    } finally {
      setLoadingId(null);
      fetchFacturas()
      .then(setFacturas)
      .catch(() => setError("No se pudieron cargar las facturas."));

      setTimeout(() => {
        setMensaje(null);
      }, 2000);
    }
  };

  if (error) return <p>{error}</p>;


  const EstadoFactura = ({ factura }: { factura: Factura }) => {
    if (factura.estado === 0) {
      return <p>Primer Recordatorio</p>;
    }
    if (factura.estado === 1) {
      return <p>Segundo Recordatorio</p>;
    }
    if (factura.estado === 2) {
      return <p>Desactivado</p>;
    }
  };
   
  return (
    <div className="container">
      <h1>Listado de Facturas</h1>
      <ul>
        {facturas.length === 0 ? (
          <p>No hay facturas disponibles.</p>
        ) : (
          <table className="facturas-table">
            <thead>
              <tr>
                <th>Código</th>
                <th>Cliente</th>
                <th>Ciudad</th>
                <th>NIT</th>
                <th>Total</th>
                <th>Estado</th>
                <th>Acciones</th>
              </tr>
            </thead>
            <tbody>
              {facturas.map((factura) => (
                <tr key={factura.id}>
                  <td>{factura.codigoFactura}</td>
                  <td>{factura.cliente.nombre}</td>
                  <td>{factura.ciudad}</td>
                  <td>{factura.nit}</td>
                  <td>${factura.totalFactura.toFixed(2)}</td>
                  <td><EstadoFactura factura={factura} /></td>
                  <td className="actions">
                    <div>
                      <Link to={`/facturas/${factura.id}`}>Ver detalles</Link>
                      <button
                        onClick={() => handleEnviarRecordatorio(factura)}
                        disabled={loadingId === factura.id}
                        style={{cursor: "pointer"}}
                      >
                        {loadingId === factura.id ? "Enviando..." : "Enviar recordatorio"}
                      </button>
                    </div>
                    {mensaje && mensaje.id === factura.id && <p>{mensaje.text}</p>}
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        )}
      </ul>
    </div>
  );
};

export default FacturasList;