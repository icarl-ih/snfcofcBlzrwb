// wwwroot/js/exporter.js

async function ensureDeps() {
  // Carga html2canvas
  if (typeof html2canvas === "undefined") {
    await new Promise((res, rej) => {
      const s = document.createElement("script");
      s.src = "https://cdnjs.cloudflare.com/ajax/libs/html2canvas/1.4.1/html2canvas.min.js";
      s.onload = res; s.onerror = rej; document.head.appendChild(s);
    });
  }
  // Carga jsPDF
  if (typeof window.jspdf === "undefined") {
    await new Promise((res, rej) => {
      const s = document.createElement("script");
      s.src = "https://cdnjs.cloudflare.com/ajax/libs/jspdf/2.5.1/jspdf.umd.min.js";
      s.onload = res; s.onerror = rej; document.head.appendChild(s);
    });
  }
}

async function getCanvas(elementId) {
  await ensureDeps();
  const el = document.getElementById(elementId);
  if (!el) throw new Error(`No se encontró #${elementId}`);
  // Aumenta scale si deseas más nitidez (2 o 3)
  return await html2canvas(el, { scale: 2, useCORS: true });
}

// === Exportar PNG (WASM: descarga directa) ===
export async function downloadPng(elementId, filename = "captura.png") {
  const canvas = await getCanvas(elementId);
  const dataUrl = canvas.toDataURL("image/png");
  const a = document.createElement("a");
  a.href = dataUrl;
  a.download = filename;
  a.click();
}

// === Exportar PNG (MAUI: devuelve base64 sin encabezado data:) ===
export async function capturePngBase64(elementId) {
  const canvas = await getCanvas(elementId);
  const dataUrl = canvas.toDataURL("image/png");
  return dataUrl.split(",")[1]; // base64 puro
}

// === Exportar PDF (WASM: descarga directa) ===
export async function downloadPdf(elementId, filename = "captura.pdf") {
  await ensureDeps();
  const canvas = await getCanvas(elementId);
  const imgData = canvas.toDataURL("image/png");

  const { jsPDF } = window.jspdf;
  const pdf = new jsPDF("p", "mm", "a4");

  const imgProps = pdf.getImageProperties(imgData);
  const pdfW = pdf.internal.pageSize.getWidth();
  const pdfH = (imgProps.height * pdfW) / imgProps.width;

  pdf.addImage(imgData, "PNG", 0, 0, pdfW, pdfH);
  pdf.save(filename);
}

// === Exportar PDF (MAUI: devuelve base64) ===
export async function capturePdfBase64(elementId) {
  await ensureDeps();
  const canvas = await getCanvas(elementId);
  const imgData = canvas.toDataURL("image/png");

  const { jsPDF } = window.jspdf;
  const pdf = new jsPDF("p", "mm", "a4");

  const imgProps = pdf.getImageProperties(imgData);
  const pdfW = pdf.internal.pageSize.getWidth();
  const pdfH = (imgProps.height * pdfW) / imgProps.width;

  pdf.addImage(imgData, "PNG", 0, 0, pdfW, pdfH);

  // Devuelve el PDF como base64 (sin encabezado data:)
  const pdfDataUri = pdf.output("datauristring"); // "data:application/pdf;base64,...."
  return pdfDataUri.split(",")[1];
}
