import React from 'react'
import { useState, useEffect } from "react";
import useAxiosPrivate from "../../hooks/useAxiosPrivate";
import { useNavigate, useLocation } from "react-router-dom";

function PayForOrder() {
  const [orders, setOrders] = useState([])
  const [order, setOrder] = useState([])
  const axiosPrivate = useAxiosPrivate();
  const navigate = useNavigate();
  const location = useLocation();

  async function updateStatus() {
    // store the states in the form data
    var orderId = order ? order : -1;

    if (orderId !== -1) {
      try {
        // make axios post request
        await axiosPrivate.put('api/Order/' + orderId + '/paid',
          JSON.stringify({ }),
          {
            headers: { 'Content-Type': 'application/json' },
            withCredentials: true
          }
        );
        alert("Opłacono!")
      } catch (error) {
        console.log(error)
      }
    }
  }

  useEffect(() => {
    const getOrders = async () => {
      try {
        const response = await axiosPrivate.get('/api/Order')
        var notPaidOrders = response.data.filter(o => o.paid === false)
        console.log(notPaidOrders)
        setOrders(notPaidOrders)
      } catch (err) {
        console.error(err)
        navigate('/log', { state: { from: location }, replace: true })
      }
    }

    getOrders()
  }, [])

  return (
    <>

      <div>Nieopłacone zamówienia:</div>
      {
        orders ? (
          orders.length > 0 ? (
            <form onSubmit={() => updateStatus()}>
              <label>Zamówienie:</label><br></br>
              <select name="order" onChange={(e) => setOrder(e.target.value)}>
                <option value="-1">Wybierz zamówenie</option>
                {orders.map((o) =>
                  <option value={o.id}>{o.id}, {o.customer.name} {o.customer.surname}, {o.customer.email}</option>)}
              </select><br></br>
              <input type="submit" value="Opłać"></input>
            </form>
          )
          : "Brak nieopłaconych zamówień."
        )
          : <p>Ładowanie...</p >
      }

    </>
  )
}

export default PayForOrder