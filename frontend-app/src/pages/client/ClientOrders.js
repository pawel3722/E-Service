import React from 'react'
import { useState, useEffect } from "react";
import useAxiosPrivate from "../../hooks/useAxiosPrivate";
import { useNavigate, useLocation } from "react-router-dom";

function ClientOrders() {
  const [serviceTypes, setServiceTypes] = useState([])
  const [orders, setOrders] = useState()
  const [reviews, setReview] = useState([])
  const [formValue, setformValue] = React.useState({
    'rating' : 0,
    'comment' : ''
  });
  const axiosPrivate = useAxiosPrivate();
  const navigate = useNavigate();
  const location = useLocation();

  async function addReview(id) {
    // store the states in the form data
    const loginFormData = new FormData();
    loginFormData.append("rating", formValue.rating)
    loginFormData.append("comment", formValue.comment)

    var rating = formValue.rating
    var comment = formValue.comment
    var orderId = id

    if(rating > 0 && rating < 6 && comment !== '') {
      try {
        // make axios post request
       await axiosPrivate.post('api/Review',
          JSON.stringify({ rating, comment, orderId }),
          {
              headers: { 'Content-Type': 'application/json' },
              withCredentials: true
          }
        );
      } catch(error) {
        console.log(error)
      }
    }
  }

  async function updateReview(oid,rid) {
    // store the states in the form data
    var rating = formValue.rating
    var comment = formValue.comment
    var orderId = oid

    if(rating > 0 && rating < 6 && comment !== '') {
      try {
        // make axios post request
        await axiosPrivate.put('api/Review/' + rid,
          JSON.stringify({ rating, comment, orderId }),
          {
              headers: { 'Content-Type': 'application/json' },
              withCredentials: true
          }
        );
      } catch(error) {
        console.log(error)
      }
    }
  }

  async function handleChange(event) {
    setformValue({
      ...formValue,
      [event.target.name]: event.target.value
    });
    console.log('handle')
  }  

  useEffect(() => {
    const getReview = async (id) => {
      try {
        const response = await axiosPrivate.get('/api/ApplicationUser/me/customer-orders/' + id + '/review')
        console.log(response.data)  
        setReview(previousState => [...previousState,response.data]);
      } catch (err) {}
  }

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
          response.data.map((o) => getReview(o.id)) 
          setOrders(response.data)
        } catch (err) {
          console.error(err)
          navigate('/log', { state: { from: location }, replace: true })
        }
      }

    getOrders()
    getServiceTypes()

    
  }, [])

  console.log(reviews)

 var myOrders = []
 var myReviews = []
 var myServiceTypes = []

 if(orders) {
  myOrders = orders
}

  if(reviews ) {
    myReviews = reviews   
  }
  
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
                    { !myReviews ? "Ładowanie" : myReviews.find((r) => r.orderId === o.id) ?
                    (
                      <p>
                      Ocena: {myReviews.find((r) => r.orderId === o.id).rating}<br></br>              
                      Komentarz: {myReviews.find((r) => r.orderId === o.id).comment}
                      </p>
                    ) : (
                      <form onSubmit={() => addReview(o.id)}>
                      <label>Ocena:</label><br></br>
                      <input  type="number" 
                              id="rating" 
                              name="rating" 
                              onInput={handleChange}
                      /><br></br>
                      <label>Komentarz:</label><br></br>
                      <input  type="text" 
                              id="comment" 
                              name="comment" 
                              onInput={handleChange}
                      /><br></br>
                      <input type="submit" value="Zapisz"></input>
                    </form>
                    )}
                    </p>
                  )) 
                : <p>Ładowanie...</p>
            }
      
      </>
  )
}

export default ClientOrders