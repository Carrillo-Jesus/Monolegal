import { useEffect, useState } from "react";
import Factura from "../types/Factura";
import { useParams } from "react-router-dom";
import { fetchFacturaById } from "../services/facturasService";

const FacturaDetail = () => {
  const { id } = useParams<{ id: string }>();
  const [factura, setFactura] = useState<Factura | null>(null);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    if (id) {
      fetchFacturaById(id)
        .then(setFactura)
        .catch(() => setError("No se pudo cargar la factura."));
    }
  }, [id]);

  if (error) return <p>{error}</p>;
  if (!factura) return <p>Cargando...</p>;

  
  return (
    <div className="factura-container">
      <div className="factura-card">
        <h1 className="factura-title">Factura #{factura.codigoFactura}</h1>
        
        <div className="factura-info">
          <p><strong>Cliente:</strong> {factura.cliente.nombre}</p>
          <p><strong>Ciudad:</strong> {factura.ciudad}</p>
          <p><strong>NIT:</strong> {factura.nit}</p>
          <p><strong>Total:</strong> <span className="total">${factura.totalFactura.toFixed(2)}</span></p>
          <p><strong>Estado:</strong> 
            <span className={`estado`}>
              {
                  factura.estado === 0 ? <span>Primer Recordatorio</span>
                : factura.estado === 1 ? <span>Segundo Recordatorio</span>
                : factura.estado === 2 ? <span>Desactivado</span>
                : <span>Desconocido</span>
              }
            </span>
          </p>
          {factura.pagada && (
            <p><strong>Fecha de Pago:</strong> {factura.fechaPago}</p>
          )}
        </div>
      </div>
    </div>
  );
};

export default FacturaDetail;