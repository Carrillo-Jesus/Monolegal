import './App.css'
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import FacturasList from "./pages/Facturas";
import FacturaDetail from "./pages/DetallesFacturas";

function App() {
  return (
    <Router>
    <Routes>
      <Route path="/" element={<FacturasList />} />
      <Route path="/facturas/:id" element={<FacturaDetail />} />
    </Routes>
  </Router>
  )
}

export default App
