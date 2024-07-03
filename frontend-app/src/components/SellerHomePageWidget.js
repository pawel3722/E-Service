import React from 'react'
import useAxiosPrivate from "../hooks/useAxiosPrivate";
import { useState, useEffect } from "react";
import { useNavigate, useLocation } from "react-router-dom";


function SellerHomePageWidget() {
    const axiosPrivate = useAxiosPrivate();
    const navigate = useNavigate();
    const location = useLocation();

    useEffect(() => {
    },[])

      return (
        <>
            <div>Widżet strony głównej w przygotowaniu.</div>
            <button onClick={() => navigate('/user/new-order')}>Nowe zamówienie</button>
            <button onClick={() => navigate('/user/pay-for-order')}>Opłacenie zamówienia</button>
            <button onClick={() => navigate('/user/deliver-order')}>Odbiór zamówienia</button>
        </>
      )
}

export default SellerHomePageWidget