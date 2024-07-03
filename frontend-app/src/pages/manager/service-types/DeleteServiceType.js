import React from 'react'
import { useState, useEffect } from "react";
import useAxiosPrivate from "../../../hooks/useAxiosPrivate";
import { useNavigate, useLocation, useParams } from "react-router-dom";

function DeleteServiceType() {
  const [serviceName, setServiceName] = useState()
  const [serviceDeviceType, setServiceDeviceType] = useState()
  const [serviceMinPrice, setServiceMinPrice] = useState()
  const [serviceMaxPrice, setServiceMaxPrice] = useState()
  const navigate = useNavigate();
  const location = useLocation();
  const { id } = useParams()

  const axiosPrivate = useAxiosPrivate();

  async function deleteServiceType() {
    try {
      // make axios post request
      await axiosPrivate.delete('/api/ServiceType/' + id)
      alert("Usunięto usługę!")
    } catch (error) {
      console.log(error)
    }
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
        <form onSubmit={() => deleteServiceType()}>
          <label>Nazwa: {serviceName}</label><br></br>
          <label>Urządzenie: {serviceDeviceType}</label><br></br>
          <label>Cena minimalna: {serviceMinPrice}</label><br></br>
          <label>Cena maksymalna: {serviceMaxPrice}</label><br></br>
          <input type="submit" value="Usuń"></input>
        </form>
      }

    </>
  )
}

export default DeleteServiceType