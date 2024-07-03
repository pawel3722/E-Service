import React from 'react'
import { useState, useEffect } from "react";
import useAxiosPrivate from "../../hooks/useAxiosPrivate";
import { useNavigate, useLocation } from "react-router-dom";

function AssignServicesToOrder() {
  const [serviceTypes, setServiceTypes] = useState([])
  const [orders, setOrders] = useState([])
  const [reviews, setReview] = useState([])
  const [formValue, setformValue] = React.useState({
    'rating': 0,
    'comment': ''
  });
  const axiosPrivate = useAxiosPrivate();
  const navigate = useNavigate();
  const location = useLocation();

  async function addReview(id) {
    var rating = formValue.rating
    var comment = formValue.comment
    var orderId = id

    if (rating >= 1 && rating <= 5 && comment !== '') {
      try {
        // make axios post request
        await axiosPrivate.post('api/Review',
          JSON.stringify({ rating, comment, orderId }),
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

  async function handleChange(event) {
    setformValue({
      ...formValue,
      [event.target.name]: event.target.value
    });
  }

  useEffect(() => {
    const getServiceTypes = async () => {
      try {
        const response = await axiosPrivate.get('/api/ServiceType/')
        console.log(response.data)
        setServiceTypes(response.data)
      } catch (err) {
        console.error(err)
        navigate('/log', { state: { from: location }, replace: true })
      }
    }

    const getOrders = async () => {
      try {
        const response = await axiosPrivate.get('/api/ApplicationUser/me/manager-orders')
        console.log(response.data)
        setOrders(response.data)
      } catch (err) {
        console.error(err)
        navigate('/log', { state: { from: location }, replace: true })
      }
    }

    getOrders()
    getServiceTypes()
  }, [])

  return (
    <>

      <div>Moje zamówienia:</div>
      {
        orders
          ? (
            orders.map((o) =>
              <p>
                Numer: {o.id}<br></br>
                Data: {o.date.split('T')[0]}<br></br>
                Opłacone: {o.paid ? "tak" : "nie"}<br></br>
                Status: {o.status === 0 ? "Przyjęto do realizacji"
                  : o.status === 1 ? "Przypisano menadżera"
                    : o.status === 2 ? "Ukończono ekspertyzę"
                      : o.status === 3 ? "Zlecono wykonanie działań"
                        : o.status === 4 ? "Naprawiono" : "Odebrano"}<br></br>
                Usługi: {o.services.map((s) =>
                  <li>
                    Usługa: {serviceTypes.find((st) => st.id === s.serviceTypeId) ? serviceTypes.find((st) => st.id === s.serviceTypeId).name : ""} ,
                    Cena: {s.servicePrice},
                    Status: {s.status === 0 ? "Utworzono"
                      : s.status === 1 ? "Przypisano pracownika"
                        : s.status === 2 ? "Oczekiwanie na część" : "Ukończono"}
                  </li>
                )}
                <button onClick={ () => navigate('/user/new-service/' + o.id, { state: { from: location }, replace: true }) }>Dodaj</button>
              </p>
            ))
          : <p>Ładowanie...</p>
      }

    </>
  )
}

export default AssignServicesToOrder