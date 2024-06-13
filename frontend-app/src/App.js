import { Route, Routes } from 'react-router-dom';
import './App.css';
import Uslugi from './pages/Uslugi';
import Kontakt from './pages/Kontakt';
import Log from './pages/Log';
import Home from './pages/Home';
import AdminPage from './pages/AdminPage';
import ProtectedRoutes from './context/ProtectedRoutes';
import PersistLogin from './components/PersistLogin';

function App() {
  return (
    <div className="App">
      <Routes>
        <Route path="/" element={<Home />} />
        <Route path="home" element={<Home />} />
        <Route path="uslugi" element={<Uslugi />} />
        <Route path="kontakt" element={<Kontakt />} />
        <Route path="log" element={<Log />} />

        <Route element={<PersistLogin />}>
          <Route element={<ProtectedRoutes />} >
            <Route path='admin' element={<AdminPage />} />
          </Route>
        </Route>

      </Routes>
    </div>
  );
}

export default App;
