import React from 'react'
import { useState, useEffect } from "react";
import useAxiosPrivate from "../../hooks/useAxiosPrivate";
import { useNavigate, useLocation } from "react-router-dom";

function DeliverOrder() {
  const [orders, setOrders] = useState([])
  const axiosPrivate = useAxiosPrivate();
  const navigate = useNavigate();
  const location = useLocation();

  async function updateStatus(id) {
    // store the states in the form data
    var status = 5

      try {
        // make axios post request
        await axiosPrivate.put('api/Order/' + id + '/status',
          JSON.stringify({ status }),
          {
            headers: { 'Content-Type': 'application/json' },
            withCredentials: true
          }
        );
        alert('Zaktualizowano status!')
      } catch (error) {
        console.log(error)
        alert(error)
      }
  }

  useEffect(() => {
    const getOrders = async () => {
      try {
        const response = await axiosPrivate.get('/api/Order')
        var finishedOrders = response.data.filter(o => o.status === 4)
        console.log(finishedOrders)
        setOrders(finishedOrders)
      } catch (err) {
        console.error(err)
        navigate('/log', { state: { from: location }, replace: true })
      }
    }

    getOrders()
  }, [])

  return (
    <>

      <div>Ukończone zamówienia:</div>
      {
       orders
       ? (
         orders.map((o) =>
           <p>
             Numer: {o.id}<br></br>
             Data: {o.date.split('T')[0]}<br></br>
             Opłacone: {o.paid ? "tak" : "nie"}<br></br>
             <button onClick={o.paid ? () => updateStatus(o.id) : alert('Proszę opłacić zamówienie!')}>Odebrano</button>
           </p>
         ))
       : <p>Ładowanie...</p>
      }

    </>
  )
}

export default DeliverOrder