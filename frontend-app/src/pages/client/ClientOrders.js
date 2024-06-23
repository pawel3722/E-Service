import React from 'react'
import { useState, useEffect } from "react";
import useAxiosPrivate from "../../hooks/useAxiosPrivate";
import { useNavigate, useLocation } from "react-router-dom";

// function getServiceType(id, axiosPrivate, location, navigate) {
//   try {
//     const response = axiosPrivate.get('/api/ServiceType/' + id)
//     console.log(response.data)
//     return response.data
//   } catch (err) {
//     console.error(err)
//     navigate('/log', { state: { from: location }, replace: true })
//   }
// }

function ClientOrders() {
  const [serviceTypes, setServiceTypes] = useState([])
  const [orders, setOrders] = useState()
  const [reviews, setReview] = useState([])
  const axiosPrivate = useAxiosPrivate();
  const navigate = useNavigate();
  const location = useLocation();

  const getReview = async (id) => {
      try {
        const response = await axiosPrivate.get('/api/ApplicationUser/me/customer-orders/' + id + '/review')
        // console.log(response.data)  
        setReview([
          ...reviews,
          response.data
        ]);
      } catch (err) {}
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
          const response = await axiosPrivate.get('/api/ApplicationUser/me/customer-orders')
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

 var myOrders = []
 var myReviews = []

 if(reviews ) {
  myReviews = reviews   
}

  if(orders) {
    myOrders = orders
    orders.map((o) => getReview(o.id))
  }



  var myServiceTypes = []
  if(serviceTypes) {
    myServiceTypes = serviceTypes
  }

  return (
    <>
    
    <div>Moje zamówienia:</div>
            {
                myOrders.length > 0
                ? (
                  myOrders.map((o) => 
                    <p>
                    Numer: {o.id}<br></br> 
                    Data: {o.date.split('T')[0]}<br></br> 
                    Opłacone: {o.paid ? "tak" : "nie" }<br></br>
                    Status: { o.status === 0 ? "Przyjęto do realizacji" 
                            : o.status === 1 ? "Przypisano menadżera" 
                            : o.status === 2 ? "Ukończono ekspertyzę"
                            : o.status === 3 ? "Zlecono wykonanie działań"
                            : o.status === 4 ? "Naprawiono" : "Odebrano"}<br></br>
                    Usługi: {o.services.map((s) =>                   
                      <li>
                        Usługa: { myServiceTypes.find((st) => st.id === s.serviceTypeId) ? myServiceTypes.find((st) => st.id === s.serviceTypeId).name : "" } , 
                        Cena: {s.servicePrice}, 
                        Status: { o.status === 0 ? "Utworzono" 
                        : o.status === 1 ? "Przypisano pracownika" 
                        : o.status === 2 ? "Oczekiwanie na część" : "Ukończono"}
                      </li>
                    )}
                    Ocena: { !myReviews ? "Ładowanie" : myReviews.find((r) => r.orderId === o.id) ? myReviews.find((r) => r.orderId === o.id).rating : "Brak"} <br></br>
                    Komentarz: { !myReviews ? "Ładowanie" : myReviews.find((r) => r.orderId === o.id) ? myReviews.find((r) => r.orderId === o.id).comment : "Brak"}<br></br>
                    </p>
                ))
                : <p>Ładowanie...</p>
            }
      
      </>
  )
}

export default ClientOrders