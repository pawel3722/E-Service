import React from 'react'
import './Kontakt.css'

function Kontakt() {
  return (
    <div className='main_kontakt'>
      <section className='dane_firmy'>
        <h2>Dane firmy</h2>
        <ul>
          <li>Tech Solutions Sp. z o.o.</li>
          <li>ul. Nowoczesna 10</li>
          <li>00-123 Warszawa</li>
          <li>Polska</li>
          <li>NIP: 123-456-78-90</li>
          <li>REGON: 987654321</li>
          <li>KRS: 0000123456</li>
        </ul>
      </section>

      <section className='kontakt'>
        <h2>Kontakt</h2>
        <h3>Telefon:</h3>
        <ul>
          <li>Główny: +48 22 123 45 67</li>
          <li>Dział Sprzedaży: +48 22 123 45 68</li>
          <li>Wsparcie Techniczne: +48 22 123 45 69</li>
        </ul>
        <h3>E-mail</h3>
        <ul>
          <li>Ogólny: kontakt@techsolutions.pl</li>
          <li>Dział Sprzedaży: sprzedaz@techsolutions.pl</li>
          <li>Wsparcie Techniczne: wsparcie@techsolutions.pl</li>
        </ul>
      </section>

      <section className='godziny_pracy'>
        <h2>Godziny pracy</h2>
        <ul>
          <li>Poniedziałek - Piątek: 9:00 - 17:00</li>
          <li>Sobota - Niedziela: Zamknięte</li>
        </ul>
      </section>

      <section className='social_media'>
        <h2>Media społecznościowe</h2>
        <ul>
          <li>Facebook</li>
          <li>Linkedin</li>
          <li>Twitter</li>
          <li>Instagram</li>
        </ul>
      </section>

      <div className='lokalizacja'>
        <iframe
          title={"Lokalizajca firmy"}
          src="https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d2549.160016298861!2d18.67788482468643!3d50.28894082321203!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x471131023ad594ed%3A0x24ca691d97f386a1!2sAkademicka%2016%2C%2044-100%20Gliwice!5e0!3m2!1spl!2spl!4v1716040136777!5m2!1spl!2spl"
          width="400"
          height="400"
          allowFullScreen=""
          loading="lazy"
          referrerPolicy="no-referrer-when-downgrade">
        </iframe>
      </div>

    </div>
  )
}

export default Kontakt
