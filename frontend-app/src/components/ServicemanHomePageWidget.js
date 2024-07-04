import React from 'react'
import useAxiosPrivate from "../hooks/useAxiosPrivate";
import { useState, useEffect } from "react";
import { useNavigate, useLocation } from "react-router-dom";


function ServicemanHomePageWidget() {
    const axiosPrivate = useAxiosPrivate();
    const navigate = useNavigate();
    const location = useLocation();

    useEffect(() => {
    },[])

      return (
        <>
            <div>Widżet strony głównej w przygotowaniu.</div>
            <button onClick={() => navigate('/user/serviceman-services')}>Moje usługi</button>
        </>
      )
}

export default ServicemanHomePageWidget