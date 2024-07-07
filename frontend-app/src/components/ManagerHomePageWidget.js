import React from 'react'
import useAxiosPrivate from "../hooks/useAxiosPrivate";
import { useState, useEffect } from "react";
import { useNavigate, useLocation } from "react-router-dom";


function ManagerHomePageWidget() {
    const axiosPrivate = useAxiosPrivate();
    const navigate = useNavigate();
    const location = useLocation();

    useEffect(() => {
    },[])

      return (
        <>
            <div>Widżet strony głównej w przygotowaniu.</div>
            <button onClick={() => navigate('/user/assign-manager-to-order', { state: { from: location }, replace: true })}>Przypisz menedżera</button>
            <button onClick={() => navigate('/user/assign-services-to-order', { state: { from: location }, replace: true })}>Zarządzaj zamówieniami</button>
            <button onClick={() => navigate('/user/assign-worker-to-service', { state: { from: location }, replace: true })}>Zarządzaj usługami</button><br></br>
            <button onClick={() => navigate('/user/service-types', { state: { from: location }, replace: true })}>Typy usług</button>
            <button onClick={() => navigate('/user/models', { state: { from: location }, replace: true })}>Modele</button>
            <button onClick={() => navigate('/user/parts', { state: { from: location }, replace: true })}>Części</button>
        </>
      )
}

export default ManagerHomePageWidget