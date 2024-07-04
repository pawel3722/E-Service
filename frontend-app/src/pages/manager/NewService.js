import React from 'react'
import { useState, useEffect } from "react";
import useAxiosPrivate from "../../hooks/useAxiosPrivate";
import { useParams, useNavigate, useLocation } from "react-router-dom";

function NewService() {
  const [serviceTypes, setServiceTypes] = useState([])
  const [serviceType, setServiceType] = useState()
  const [minPrice, setMinPrice] = useState()
  const [maxPrice, setMaxPrice] = useState()
  const [price, setPrice] = useState()
  const { id } = useParams()
  const navigate = useNavigate();
  const location = useLocation();

  const axiosPrivate = useAxiosPrivate();

  async function createService() {
    var serviceTypeId = serviceType ? serviceType : -1
    var orderId = id
    var servicePrice = price ? price : -1

    if (serviceTypeId !== -1 && servicePrice !== -1) {
      try {
        // make axios post request
        await axiosPrivate.post('/api/Service',
          JSON.stringify({ serviceTypeId, orderId, servicePrice }),
          {
            headers: { 'Content-Type': 'application/json' },
            withCredentials: true
          }
        )
        alert("Dodano usługę!")
      } catch (error) {
        console.log(error)
        alert(error)
      }
    }
    else
      alert("Proszę wprowadzić poprawne dane!")

  }

  async function handleUpdate(value) {
    setServiceType(value)
    if (value !== -1) {
      try {
        const response = await axiosPrivate.get('/api/ServiceType/' + value)
        console.log(response.data)
        setMinPrice(response.data.minPrice)
        setMaxPrice(response.data.maxPrice)
        setPrice(response.data.minPrice)
      } catch (err) {
        console.error(err)
        navigate('/log', { state: { from: location }, replace: true })
      }
    }
  }

  useEffect(() => {

    const getServiceTypes = async () => {
      try {
        const response = await axiosPrivate.get('/api/ServiceType')
        console.log(response.data)
        setServiceTypes(response.data)
      } catch (err) {
        console.error(err)
        navigate('/log', { state: { from: location }, replace: true })
      }
    }

    getServiceTypes()
    setMinPrice(0)
    setMaxPrice(0)
  }, [])

  return (
    <>
      <div>Dodaj usługę</div>

      {
        serviceTypes ? (
          <form onSubmit={() => createService()}>
            <label>Rodzaj usługi:</label><br></br>
            <select name="serviceType" onChange={(e) => handleUpdate(e.target.value)}>
              <option value="-1">Wybierz rodzaj usługi</option>
              {serviceTypes.map((s) =>
                <option value={s.id}>{s.name}, {s.deviceType}</option>)}
            </select><br></br>
            <label>Cena:</label><br></br>
            <input type="number" name="maxPrice" defaultValue={minPrice} min={minPrice} max={maxPrice} onChange={(e) => setPrice(e.target.value)} /><br></br>
            <input type="submit" value="Zapisz"></input>
          </form>
        )
          : <p>Ładowanie...</p>
      }
    </>
  )
}

export default NewService