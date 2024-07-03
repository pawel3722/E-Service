import React from 'react'
import { useState, useEffect } from "react";
import useAxiosPrivate from "../../../hooks/useAxiosPrivate";
import { useNavigate, useLocation, useParams } from "react-router-dom";

function EditServiceType() {
  const [serviceName, setServiceName] = useState()
  const [serviceDeviceType, setServiceDeviceType] = useState()
  const [serviceMinPrice, setServiceMinPrice] = useState()
  const [serviceMaxPrice, setServiceMaxPrice] = useState()
  const navigate = useNavigate();
  const location = useLocation();
  const { id } = useParams()

  const axiosPrivate = useAxiosPrivate();

  async function editServiceType() {
    var name = serviceName ? serviceName : ''
    var deviceType = serviceDeviceType ? serviceDeviceType : ''
    var minPrice = serviceMinPrice ? Number(serviceMinPrice) : 0
    var maxPrice = serviceMaxPrice ? Number(serviceMaxPrice) : 0

    if (name !== '' && deviceType !== '' && maxPrice >= minPrice) {
      try {
        // make axios post request
        await axiosPrivate.put('/api/ServiceType/' + id,
          JSON.stringify({ name, minPrice, maxPrice, deviceType }),
          {
            headers: { 'Content-Type': 'application/json' },
            withCredentials: true
          }
        )
        alert("Dodano usługę!")
      } catch (error) {
        console.log(error)
      }
    }
    else
      alert("Proszę wprowadzić poprawne dane!")

  }

  useEffect(() => {
    const getServiceType = async () => {
      try {
        const response = await axiosPrivate.get('/api/ServiceType/' + id)
        console.log(response.data)
        setServiceName(response.data.name)
        setServiceDeviceType(response.data.deviceType)
        setServiceMinPrice(response.data.minPrice)
        setServiceMaxPrice(response.data.maxPrice)
      } catch (err) {
        console.error(err)
        navigate('/log', { state: { from: location }, replace: true })
      }
    }
    getServiceType()
  }, [])

  return (
    <>
      <div>Dodaj usługę</div>
      {
        <form onSubmit={() => editServiceType()}>
          <label>Nazwa:</label><br></br>
          <input type="text" name="name" defaultValue={serviceName} onChange={(e) => setServiceName(e.target.value)} /><br></br>
          <label>Urządzenie:</label><br></br>
          <input type="text" name="device" defaultValue={serviceDeviceType} onChange={(e) => setServiceDeviceType(e.target.value)} /><br></br>
          <label>Cena minimalna:</label><br></br>
          <input type="number" name="minPrice" defaultValue={serviceMinPrice} min="0" onChange={(e) => setServiceMinPrice(e.target.value)} /><br></br>
          <label>Cena maksymalna:</label><br></br>
          <input type="number" name="maxPrice" defaultValue={serviceMaxPrice} min="0" onChange={(e) => setServiceMaxPrice(e.target.value)} /><br></br>
          <input type="submit" value="Zapisz"></input>
        </form>
      }

    </>
  )
}

export default EditServiceType