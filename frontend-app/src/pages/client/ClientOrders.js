import React from 'react'
import { useState, useEffect } from "react";
import useAxiosPrivate from "../../hooks/useAxiosPrivate";
import { useNavigate, useLocation } from "react-router-dom";

function ClientOrders() {
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

  // async function updateReview(oid, rid) {
  //   // store the states in the form data
  //   var rating = formValue.rating
  //   var comment = formValue.comment
  //   var orderId = oid

  //   if (rating > 0 && rating < 6 && comment !== '') {
  //     try {
  //       // make axios post request
  //       await axiosPrivate.put('api/Review/' + rid,
  //         JSON.stringify({ rating, comment, orderId }),
  //         {
  //           headers: { 'Content-Type': 'application/json' },
  //           withCredentials: true
  //         }
  //       );
  //     } catch (error) {
  //       console.log(error)
  //     }
  //   }
  // }

  async function handleChange(event) {
    setformValue({
      ...formValue,
      [event.target.name]: event.target.value
    });
  }

  useEffect(() => {
    const getReview = async (id) => {
      try {
        const response = await axiosPrivate.get('/api/ApplicationUser/me/customer-orders/' + id + '/review')
        console.log(response.data)
        setReview(previousState => [...previousState, response.data]);
      } catch (err) { }
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

  return (
    <>

      <div>Moje zamówienia:</div>
      {
        orders.length > 0
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
                    Status: {o.status === 0 ? "Utworzono"
                      : o.status === 1 ? "Przypisano pracownika"
                        : o.status === 2 ? "Oczekiwanie na część" : "Ukończono"}
                  </li>
                )}
                {!reviews ? "Ładowanie" : reviews.find((r) => r.orderId === o.id) ?
                  (
                    <p>
                      Ocena: {reviews.find((r) => r.orderId === o.id).rating}<br></br>
                      Komentarz: {reviews.find((r) => r.orderId === o.id).comment}
                    </p>
                  ) : o.status !== 5 ? "" : (
                    <form onSubmit={() => addReview(o.id)}>
                      <label>Ocena:</label><br></br>
                      <input type="number"
                        id="rating"
                        name="rating"
                        onInput={handleChange}
                      /><br></br>
                      <label>Komentarz:</label><br></br>
                      <input type="text"
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