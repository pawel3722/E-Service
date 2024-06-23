import React from 'react'
import { useState, useEffect } from "react";
import useAxiosPrivate from "../../hooks/useAxiosPrivate";
import { useNavigate, useLocation } from "react-router-dom";

function ClientOrders() {
  const [users, setUsers] = useState();
  const [orders, setOrders] = useState()
  const axiosPrivate = useAxiosPrivate();
  const navigate = useNavigate();
  const location = useLocation();

  useEffect(() => {
    
    const getUsers = async () => {
      try {
        const response = await axiosPrivate.get('/api/Auth/users/me')
        console.log(response.data)
        setUsers(response.data)
      } catch (err) {
        console.error(err)
        navigate('/log', { state: { from: location }, replace: true })
      }
    }

    getUsers()

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

  }, [])

  return (
    <>
    
    <div>Zamowienia w realizacji:</div>
            {
                orders
                ? (
                  orders.map((o) => {
                    <p>
                    Numer: {o.id}<br></br> 
                    Data: {o.date.split('T')[0]}<br></br> 
                    Opłacone: {o.paid ? "tak" : "nie" }<br></br>
                    Status: { o.status == 0 ? "Przyjęto do realizacji" 
                            : o.status == 1 ? "Przypisano menadżera" 
                            : o.status == 2 ? "Ukończono ekspertyzę"
                            : o.status == 3 ? "Zlecono wykonanie działań"
                            : o.status == 4 ? "Naprawiono" : "Odebrano"}<br></br>
                    Uslugi:
                     
                    </p>
                }))
                : <p>Ładowanie...</p>
            }
      
      </>
  )
}

export default ClientOrders