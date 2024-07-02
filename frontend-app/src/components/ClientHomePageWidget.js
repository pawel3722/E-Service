import React from 'react'
import useAxiosPrivate from "../hooks/useAxiosPrivate";
import { useState, useEffect } from "react";
import { useNavigate, useLocation } from "react-router-dom";


function ClientHomePageWidget() {
    const [orders, setOrders] = useState();
    const axiosPrivate = useAxiosPrivate();
    const navigate = useNavigate();
    const location = useLocation();

    useEffect(() => {
      const getOrders = async () => {
        try {
          const response = await axiosPrivate.get('/api/ApplicationUser/me/customer-orders')
          console.log(response.data)
          setOrders(response.data)
        } catch (err) {
          console.error(err)
          //navigate('/log', { state: { from: location }, replace: true })
        }
      }

    getOrders()
    },[])

    var processingOrders = []
    var finishedOrders = []
    if(orders)
      {      
        processingOrders = orders.filter((o) => o.status != 5)
        finishedOrders = orders.filter((o) => o.status == 5)
      }

      return (
        <>
            <div>Zamowienia w realizacji:</div>
            {
                processingOrders.length > 0
                ? (
                  processingOrders.map((o) => 
                    <li>Numer: {o.id}, Data: {o.date.split('T')[0]}, Opłacone: {o.paid ? "tak" : "nie" }</li>
                  ))
                : <p>Brak zamowien</p>
            }
            <div>Ukonczone zamowienia:</div>
            {
                finishedOrders.length > 0
                ? (
                  finishedOrders.map((o) => 
                    <li>Numer: {o.id}, Data: {o.date.split('T')[0]}, Opłacone: {o.paid ? "tak" : "nie" }</li>
                  ))
                : <p>Brak zamowien</p>
            }
            <button onClick={() => navigate('/user/client-orders', { state: { from: location }, replace: true })}>Moje zamowienia</button>
        </>
      )
}

export default ClientHomePageWidget