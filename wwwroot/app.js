//submits a complaint
document
  .getElementById("complaintForm")
  .addEventListener("submit", async (event) => {
    event.preventDefault();
    const description = document.getElementById("description").value;
    const response = await fetch("http://localhost:5000/complaint", {
      //failed to fetch
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify({ description }),
    });
    if (response.ok) {
        const data = await response.json();
        document.getElementById('responseMessage').innerText = `Complaint submitted successfully with ID: ${data.id}`;
    } else {
        document.getElementById('responseMessage').innerText = 'Failed to submit complaint.';
    } 
  });
//gets all the complaints
  const fetchComplaints = async () => {
    const response = await fetch("http://localhost:5000/complaint");
    if (response.ok) {
      const complaints = await response.json();
      const complaintTableBody = document.getElementById("complaintTableBody");
      complaintTableBody.innerHTML = "";
      complaints.forEach((complaint) => {
        const row = document.createElement("tr");
        row.innerHTML = `
          <td>${complaint.id}</td>
          <td>${complaint.description}</td>
          <td>${new Date(complaint.createdAt).toLocaleString()}</td>
        `;
        complaintTableBody.appendChild(row);
      });
    }
  };

  window.onload = fetchComplaints;