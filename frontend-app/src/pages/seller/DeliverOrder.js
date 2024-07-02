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
    var status = 0

    if (status >= 0 && status <= 5) {
      try {
        // make axios post request
        await axiosPrivate.put('api/Order/' + id,
          JSON.stringify({ status }),
          {
            headers: { 'Content-Type': 'application/json' },
            withCredentials: true
          }
        );
      } catch (error) {
        console.log(error)
      }
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
        orders ? (
          orders.length > 0 ? (
            "Tu będą zamówenia. Najpierw trzeba zaimplementować wcześniejsze etapy."
          )
          : "Brak zamówień."
        )
          : <p>Ładowanie...</p >
      }

    </>
  )
}

export default DeliverOrder