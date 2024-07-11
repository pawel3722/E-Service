import React from 'react'
import { useState, useEffect } from "react";
import useAxiosPrivate from "../../hooks/useAxiosPrivate";
import { useNavigate, useLocation } from "react-router-dom";
import './ClientOrders.css'
import arrow from '../../image/arrow.png'

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
        alert('Dodano ocenę!')
      } catch (error) {
        console.log(error)
        alert(error)
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
      {
        orders ? orders.map((o) => {
          return (
            <div class='order_details'>
              <div id='div1'>
                <p> Numer: {o.id} </p>
                <p> Data: {o.date.split('T')[0]} </p>
                <p> Opłacone: {o.paid ? "tak" : "nie"} </p>
                <p> Status: {
                  (() => {
                    switch (o.status) {
                      case 0:
                        return "Przyjęto do realizacji"
                      case 1:
                        return "Przypisano menadżera"
                      case 2:
                        return "Ukończono ekspertyzę"
                      case 3:
                        return "Zlecono wykonanie działań"
                      case 4:
                        return "Naprawiono"
                      default:
                        return "Odebrano"
                    }
                  })()
                } </p>
              </div>
              <h3>Wykonane usługi</h3>
              <table>
                <tr>
                  <th>Usługa</th>
                  <th>Cena</th>
                  <th>Status</th>
                </tr>
                {
                  o.services.map((s) => {
                    return (
                      <tr>
                        <td>{serviceTypes.find((st) => st.id === s.serviceTypeId) ? serviceTypes.find((st) => st.id === s.serviceTypeId).name : ""}</td>
                        <td>{s.servicePrice}</td>
                        <td>{(() => {
                          switch (s.status) {
                            case 0:
                              return "Utworzono"
                            case 1:
                              return "Przypisano pracownika"
                            case 2:
                              return "Oczekiwanie na część"
                            default:
                              return "Ukończono"
                          }
                        })()}</td>
                      </tr>
                    )
                  })
                }
              </table>
              {!reviews ? "Ładowanie" : reviews.find((r) => r.orderId === o.id) ?
                (
                  <div id='div2'>
                    <p>Komentarz: {reviews.find((r) => r.orderId === o.id).comment}</p>
                    <p>Ocena: {reviews.find((r) => r.orderId === o.id).rating}</p>
                  </div>
                ) : o.status !== 5 ? "" : (
                  <form onSubmit={() => addReview(o.id)}>
                    <div class='revind'>
                      <input type="text"
                        id="comment"
                        name="comment"
                        onInput={handleChange}
                        placeholder='Wpisz komentarz'
                        className='comment'
                      />
                      <label>Ocena:</label>
                      <input type="number"
                        id="rating"
                        name="rating"
                        onInput={handleChange}
                        className='grade'
                      />
                    </div>
                    <button className='sendBtn'>
                      <img src={arrow} alt='' />
                    </button>
                  </form>
                )}
            </div>
          )
        })
          : <p>Ładowanie...</p>
      }

    </>
  )
}

export default ClientOrders