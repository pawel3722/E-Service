import React from 'react'
import { useState, useEffect } from "react";
import useAxiosPrivate from "../../hooks/useAxiosPrivate";
import { useNavigate, useLocation } from "react-router-dom";

function PayForOrder() {
  const [orders, setOrders] = useState([])
  const [order, setOrder] = useState([])
  const [totalPrice, setTotalPrice] = useState()
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
          JSON.stringify({}),
          {
            headers: { 'Content-Type': 'application/json' },
            withCredentials: true
          }
        );
        alert("Opłacono!")
      } catch (error) {
        console.log(error)
        alert(error)
      }
    }
  }

  async function handleChange(id) {
    setOrder(id)
    if (id !== '-1') {
      var currentOrder = orders.find(o => o.id === Number(id))
      var sum = 0
      currentOrder.services.map(s => sum += s.servicePrice + s.partPrice)
      setTotalPrice(sum)
    }
  }

  useEffect(() => {
    const getOrders = async () => {
      try {
        const response = await axiosPrivate.get('/api/Order')
        var notPaidOrders = response.data.filter(o => o.paid === false && o.status >= 4)
        console.log(notPaidOrders)
        setOrders(notPaidOrders)
      } catch (err) {
        console.error(err)
        navigate('/log', { state: { from: location }, replace: true })
      }
    }

    getOrders()
    setTotalPrice(0)
  }, [])

  return (
    <>

      <div>Nieopłacone zamówienia:</div>
      {
        orders ? (
          orders.length > 0 ? (
            <form onSubmit={() => updateStatus()}>
              <label>Zamówienie:</label><br></br>
              <select name="order" onChange={(e) => handleChange(e.target.value)}>
                <option value="-1">Wybierz zamówienie</option>
                {orders.map((o) =>
                  <option value={o.id}>{o.id}, {o.customer.name} {o.customer.surname}, {o.customer.email}</option>)}
              </select><br></br>
              <label>Do zapłaty: {totalPrice} zł</label><br></br>
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