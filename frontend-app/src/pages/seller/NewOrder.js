import React from 'react'
import { useState, useEffect } from "react";
import useAxiosPrivate from "../../hooks/useAxiosPrivate";
import { useNavigate, useLocation } from "react-router-dom";

function NewOrder() {
  const [clients, setClients] = useState([])
  const [managers, setManagers] = useState([])
  const [client, setClient] = useState()
  const [wasPaid, setPaid] = useState()
  const [manager, setManager] = useState()
  const axiosPrivate = useAxiosPrivate();
  const navigate = useNavigate();
  const location = useLocation();

  async function createOrder() {
    // store the states in the form data
    var customerId = client ? client : -1
    var paid = wasPaid ? wasPaid : false
    var managerId = manager ? manager : -1

    console.log(paid)
    console.log(customerId)

    if (customerId !== -1) {
      if (managerId !== -1) {
        try {
          // make axios post request
          await axiosPrivate.post('api/Order',
            JSON.stringify({ paid, customerId, managerId }),
            {
              headers: { 'Content-Type': 'application/json' },
              withCredentials: true
            }
          )
          alert("Dodano zamówienie!")
        } catch (error) {
          console.log(error)
        }
      }
      else {
        try {
          // make axios post request
          await axiosPrivate.post('api/Order',
            JSON.stringify({ paid, customerId }),
            {
              headers: { 'Content-Type': 'application/json' },
              withCredentials: true
            }
          )
          alert("Dodano zamówienie!")
        } catch (error) {
          console.log(error)
        }
      }
    }
    else
      alert("Proszę wybrać klienta!")
  }

  useEffect(() => {

    const getClients = async () => {
      try {
        const response = await axiosPrivate.get('/api/ApplicationUser/roles')
        console.log(response.data)
        var newClients = response.data.find(r => r.name === 'Client').users
        setClients(newClients)
        var newManagers = response.data.find(r => r.name === 'Manager').users
        setManagers(newManagers)
      } catch (err) {
        console.error(err)
        navigate('/log', { state: { from: location }, replace: true })
      }
    }

    getClients()
  }, [])

  return (
    <>

      <div>Nowe zamówienie:</div>
      {
        clients.length > 0
          ? (
            <form onSubmit={() => createOrder()}>
              <label>Klient:</label><br></br>
              <select name="client" onChange={(e) => setClient(e.target.value)}>
                <option value="-1">Wybierz klienta</option>
                {clients.map((c) =>
                  <option value={c.id}>{c.surname} {c.name} {c.email}</option>)}
              </select><br></br>
              <div onChange={(e) => setPaid(e.target.value)}>Opłacone:<br></br>
                <input type="radio" id="yes" name="paid" value="true" />Tak<br></br>
                <input type="radio" id="no" name="paid" value="false" defaultChecked />Nie<br></br>
              </div>
              <label>Menedżer:</label><br></br>
              <select name="manager" onChange={(e) => setManager(e.target.value)}>
                <option value="-1">Brak</option>
                {managers.length > 0 ? managers.map((c) => <option value={c.id}>{c.surname} {c.name} {c.email}</option>) : ""}
              </select><br></br>
              <input type="submit" value="Zapisz"></input>
            </form>
          )
          : <p>Ładowanie...</p>
      }

    </>
  )
}

export default NewOrder