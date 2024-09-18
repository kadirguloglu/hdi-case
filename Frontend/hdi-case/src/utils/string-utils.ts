export function turkishToEnglish(text: string) {
  if (!text) text = "";
  text = text.toString();
  text = text.toUpperCase();
  text = text
    .replace(/Ğ/g, "g")
    .replace(/ğ/g, "g")
    .replace(/Ü/g, "u")
    .replace(/ü/g, "u")
    .replace(/Ş/g, "s")
    .replace(/ş/g, "s")
    .replace(/İ/g, "i")
    .replace(/i/g, "i")
    .replace(/I/g, "i")
    .replace(/ı/g, "i")
    .replace(/Ö/g, "o")
    .replace(/ö/g, "o")
    .replace(/Ç/g, "c")
    .replace(/ç/g, "c");
  return text.toUpperCase();
}
