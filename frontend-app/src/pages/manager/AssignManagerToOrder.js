import React from 'react'
import { useState, useEffect } from "react";
import useAxiosPrivate from "../../hooks/useAxiosPrivate";
import { useNavigate, useLocation } from "react-router-dom";

function AssignManagerToOrder() {
  const [orders, setOrders] = useState([])
  const [order, setOrder] = useState()
  const [managers, setManagers] = useState([])
  const [manager, setManager] = useState()
  const axiosPrivate = useAxiosPrivate();
  const navigate = useNavigate();
  const location = useLocation();

  async function updateOrder() {
    var managerId = manager ? manager : -1
    var orderId = order ? order : -1


    if (orderId !== -1) {
      if (managerId !== -1) {
        try {
          // make axios post request
          await axiosPrivate.put('api/Order/' + orderId,
            JSON.stringify({ managerId }),
            {
              headers: { 'Content-Type': 'application/json' },
              withCredentials: true
            }
          )
          alert("Dodano menedżera!")
        } catch (error) {
          console.log(error)
          alert(error)
        }
      }
      else
        alert("Proszę wybrać menedżera!")
    }
    else
      alert("Proszę wybrać zamówienie")
  }

  useEffect(() => {
    const getOrders = async () => {
      try {
        const response = await axiosPrivate.get('/api/Order')
        console.log(response.data)
        var pendingOrders = response.data.filter(o => o.status === 0)
        setOrders(pendingOrders)
      } catch (err) {
        console.error(err)
        navigate('/log', { state: { from: location }, replace: true })
      }
    }

    const getManagers = async () => {
      try {
        const response = await axiosPrivate.get('/api/ApplicationUser/roles')
        console.log(response.data)
        var newManagers = response.data.find(r => r.name === 'Manager').users
        setManagers(newManagers)
      } catch (err) {
        console.error(err)
        navigate('/log', { state: { from: location }, replace: true })
      }
    }
    getOrders()
    getManagers()
  }, [])

  return (
    <>

      <div>Przypisz menedżera:</div>
      {
        orders
          ? (
            <form onSubmit={() => updateOrder()}>
              <label>Zamówienie:</label><br></br>
              <select name="order" onChange={(e) => setOrder(e.target.value)}>
                <option value="-1">Wybierz zamówienie</option>
                {orders.map((o) =>
                  <option value={o.id}>{o.id}, {o.customer.name} {o.customer.surname}, {o.customer.email}</option>)}
              </select><br></br>
              <label>Menedżer:</label><br></br>
              <select name="manager" onChange={(e) => setManager(e.target.value)}>
                <option value="-1">Wybierz menedżera</option>
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

export default AssignManagerToOrder