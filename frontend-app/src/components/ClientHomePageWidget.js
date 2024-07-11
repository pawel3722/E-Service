import React from 'react'
import useAxiosPrivate from "../hooks/useAxiosPrivate";
import { useState, useEffect } from "react";
import { useNavigate, useLocation } from "react-router-dom";
import './ClientHomePageWidget.css'


function ClientHomePageWidget() {
  const [orders, setOrders] = useState();
  const axiosPrivate = useAxiosPrivate();
  const navigate = useNavigate();
  const location = useLocation();
  const [processingOrders, setProcessingOrders] = useState([])
  const [finishedOrders, setFinishedOrders] = useState([])

  useEffect(() => {
    const getOrders = async () => {
      try {
        const response = await axiosPrivate.get('/api/ApplicationUser/me/customer-orders')
        console.log(response.data)
        setOrders(response.data)
        if (response.data) {
          setProcessingOrders(response.data.filter((o) => o.status != 5))
          setFinishedOrders(response.data.filter((o) => o.status == 5))
        }
      } catch (err) {
        console.error(err)
        //navigate('/log', { state: { from: location }, replace: true })
      }
    }

    getOrders()

  }, [])

  // useEffect(() => {
  //   if (orders) {
  //     setProcessingOrders(orders.filter((o) => o.status != 5))
  //     setProcessingOrders(orders.filter((o) => o.status == 5))
  //   }
  // }, [orders])

  useEffect(() => {
    processingOrders.map((o) => (
      document.documentElement.style.setProperty('--circle-val', `${o.status * 20}%`)
    ))
  }, [processingOrders])

  // var processingOrders = []
  // var finishedOrders = []
  // if (orders) {
  //   processingOrders = orders.filter((o) => o.status != 5)
  //   finishedOrders = orders.filter((o) => o.status == 5)
  // }

  return (
    <>
      <h2 className='order_title'>Zamówienia w realizacji</h2>
      <div className='client_order_list'>
        {
          processingOrders.length > 0
            ? (
              processingOrders.map((o) => (
                <div key={o.id}>
                  <p>Numer: {o.id}</p>
                  <p>Data: {o.date.split('T')[0]}</p>
                  <p>Opłacone: {o.paid ? "tak" : "nie"}</p>
                  <div className='circle'>
                    <div className='inner_circle' id={o.id} value={o.status * 20}>
                      {o.status * 20}%
                    </div>
                  </div>
                </div>
              )))
            : <p>Brak zamówień</p>
        }
      </div>
      <h2 className='order_title'>Ukończone zamówienia</h2>
      <div className='client_order_list'>
        {
          finishedOrders.length > 0
            ? (
              finishedOrders.map((o) =>
                  <div key={o.id}>
                    <p>Numer: {o.id}</p>
                    <p>Data: {o.date.split('T')[0]}</p>
                    <p>Opłacone: {o.paid ? "tak" : "nie"}</p>
                  </div>
              ))
            : <p>Brak zamówień</p>
        }
      </div>
    </>
  )
}

export default ClientHomePageWidget