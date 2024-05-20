import React from 'react'
import repBat from '../image/repair-battery.png'
import './WidokUslugi.css'

function WidokUslugi(props) {

    return (

        <div>
            <h2>{props.name.name}</h2>
            <img src={repBat} alt='pc' />
            <p>Cena:</p>
            <p>{props.name.minPrice}zł - {props.name.maxPrice}zł</p>
        </div>
    )
}

export default WidokUslugi