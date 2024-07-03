import React from 'react'
import { useState, useEffect } from "react";
import useAxiosPrivate from "../../../hooks/useAxiosPrivate";

function NewServiceType() {
  const [serviceName, setServiceName] = useState()
  const [serviceDeviceType, setServiceDeviceType] = useState()
  const [serviceMinPrice, setServiceMinPrice] = useState()
  const [serviceMaxPrice, setServiceMaxPrice] = useState()

  const axiosPrivate = useAxiosPrivate();

  async function createServiceType() {
    var name = serviceName ? serviceName : ''
    var deviceType = serviceDeviceType ? serviceDeviceType : ''
    var minPrice = serviceMinPrice ? Number(serviceMinPrice) : 0
    var maxPrice = serviceMaxPrice ? Number(serviceMaxPrice) : 0

    if(name !== '' && deviceType !== '' && maxPrice >= minPrice) {
        try {
          // make axios post request
          await axiosPrivate.post('/api/ServiceType',
            JSON.stringify({ name, deviceType, minPrice, maxPrice }),
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

 

  return (
    <>
      <div>Dodaj usługę</div>
      {
        <form onSubmit={() => createServiceType()}>
        <label>Nazwa:</label><br></br>
        <input type="text" name="name" onChange={(e) => setServiceName(e.target.value)}/><br></br>
        <label>Urządzenie:</label><br></br>
        <input type="text" name="device" onChange={(e) => setServiceDeviceType(e.target.value)}/><br></br>
        <label>Cena minimalna:</label><br></br>
        <input type="number" name="minPrice" defaultValue="0" min="0" onChange={(e) => setServiceMinPrice(e.target.value)}/><br></br>
        <label>Cena maksymalna:</label><br></br>
        <input type="number" name="maxPrice" defaultValue="0" min="0" onChange={(e) => setServiceMaxPrice(e.target.value)}/><br></br>
        <input type="submit" value="Zapisz"></input>
    </form>
      }

    </>
  )
}

export default NewServiceType