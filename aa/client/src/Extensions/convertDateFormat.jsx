export default function convertDateFormat(dateStr) {
  // Split the input string into parts
  const parts = dateStr.split(' ')[0].split('.');
  const day = parts[0];
  const month = parts[1];
  const year = parts[2];

  // Construct a new date string in the "yyyy-MM-dd" format
  const formattedDate = `${year}-${month}-${day}`;
  return formattedDate;
}