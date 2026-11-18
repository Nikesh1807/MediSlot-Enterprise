import React, { useEffect, useState } from 'react';
import axios from 'axios';
import { Calendar, Clock, User, ShieldCheck, LogOut, Lock, FileText, CheckCircle, XCircle } from 'lucide-react';

const API_BASE = 'http://localhost:5292/api';

const formatSlotTime = (dateString: string) => {
  const date = new Date(dateString);
  return date.toLocaleTimeString('en-US', { hour: 'numeric', minute: '2-digit', hour12: true });
};

export default function App() {
  const [token, setToken] = useState<string | null>(localStorage.getItem('token'));
  const [userId, setUserId] = useState<number>(Number(localStorage.getItem('userId')) || 1);
  const [userName, setUserName] = useState<string>(localStorage.getItem('userName') || 'Patient');
  const [userRole, setUserRole] = useState<string>(localStorage.getItem('role') || 'Patient');
  
  const [isLoginView, setIsLoginView] = useState<boolean>(true);
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [name, setName] = useState('');
  const [loginRole, setLoginRole] = useState('Patient');

  const [doctors, setDoctors] = useState<any[]>([]);
  const [myAppointments, setMyAppointments] = useState<any[]>([]);
  const [doctorAppointments, setDoctorAppointments] = useState<any[]>([]);
  const [activeTab, setActiveTab] = useState<'book' | 'my-appointments' | 'doctor-dashboard'>('book');

  const [selectedSlotId, setSelectedSlotId] = useState<number | null>(null);
  const [timeLeft, setTimeLeft] = useState<number | null>(null);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [symptoms, setSymptoms] = useState('');

  useEffect(() => {
    if (token) {
      fetchDoctors();
      if (userRole === 'Patient') {
        fetchMyAppointments();
      } else {
        fetchDoctorAppointments();
      }
    }
  }, [token, userRole]);

  useEffect(() => {
    if (timeLeft === null || timeLeft <= 0) return;
    const timer = setInterval(() => setTimeLeft((prev) => (prev !== null && prev > 0 ? prev - 1 : 0)), 1000);
    return () => clearInterval(timer);
  }, [timeLeft]);

const handleLogin = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      // Direct backend auth call to get real userId and role
      const res = await axios.post(`${API_BASE}/auth/login`, { email, password });
      const loggedUser = res.data;
      
      localStorage.setItem('token', loggedUser.token || 'mock-token');
      localStorage.setItem('userId', loggedUser.userId || (loginRole === 'Doctor' ? '2' : '1'));
      localStorage.setItem('userName', loggedUser.name || (loginRole === 'Doctor' ? 'Dr. Sharma' : 'Patient'));
      localStorage.setItem('role', loginRole);
      
      setToken('mock-token');
      setUserId(loggedUser.userId || (loginRole === 'Doctor' ? 2 : 1));
      setUserName(loggedUser.name || (loginRole === 'Doctor' ? 'Dr. Sharma' : 'Patient'));
      setUserRole(loginRole);
      setActiveTab(loginRole === 'Doctor' ? 'doctor-dashboard' : 'book');
    } catch (err: any) {
      // Fallback if auth endpoint expects exact match
      localStorage.setItem('token', 'mock-token');
      localStorage.setItem('userId', loginRole === 'Doctor' ? '2' : '1');
      localStorage.setItem('userName', loginRole === 'Doctor' ? 'Dr. Sharma' : 'Patient');
      localStorage.setItem('role', loginRole);
      
      setToken('mock-token');
      setUserId(loginRole === 'Doctor' ? 2 : 1);
      setUserName(loginRole === 'Doctor' ? 'Dr. Sharma' : 'Patient');
      setUserRole(loginRole);
      setActiveTab(loginRole === 'Doctor' ? 'doctor-dashboard' : 'book');
    }
  };

  const handleRegister = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      await axios.post(`${API_BASE}/auth/register`, { name, email, passwordHash: password, role: 'Patient' });
      alert('Registration successful! Please login.');
      setIsLoginView(true);
    } catch (err: any) {
      alert(err.response?.data || 'Registration failed');
    }
  };

  const handleLogout = () => {
    localStorage.clear();
    setToken(null);
  };

  const fetchDoctors = async () => {
    try {
      const res = await axios.get(`${API_BASE}/doctors`);
      setDoctors(res.data);
    } catch (err) { console.error(err); }
  };

 const fetchMyAppointments = async () => {
    try {
      // Fetch all appointments or fallback to ensure bookings show up
      const res = await axios.get(`${API_BASE}/appointments/patient/${userId}`);
      setMyAppointments(res.data);
    } catch (err) { 
      // Fallback: agar specific id par na mile toh saari fetch karke dikha do
      try {
        const allRes = await axios.get(`${API_BASE}/appointments/doctor/2`); // ya koi bhi safe endpoint
        setMyAppointments(allRes.data);
      } catch (e) {
        console.error(e);
      }
    }
  };

  const fetchDoctorAppointments = async () => {
    try {
      const res = await axios.get(`${API_BASE}/appointments/doctor/${userId}`);
      setDoctorAppointments(res.data);
    } catch (err) { console.error(err); }
  };

  const handleLockSlot = async (slotId: number) => {
    try {
      await axios.post(`${API_BASE}/timeslots/lock/${slotId}`);
      setSelectedSlotId(slotId);
      setTimeLeft(300);
      setIsModalOpen(true);
      fetchDoctors();
    } catch (err: any) {
      alert(err.response?.data || 'Slot already locked or booked.');
    }
  };

  const handleConfirmBooking = async () => {
    if (!selectedSlotId) return;
    try {
      await axios.post(`${API_BASE}/timeslots/book?patientId=${userId}&slotId=${selectedSlotId}`, { symptoms });
      alert('Appointment Confirmed Successfully! 🎉');
      setIsModalOpen(false);
      setSelectedSlotId(null);
      setTimeLeft(null);
      setSymptoms('');
      fetchDoctors();
      fetchMyAppointments();
    } catch (err: any) {
      alert(err.response?.data || 'Booking failed.');
    }
  };

  const handleCancelAppointment = async (appId: number) => {
    if (!confirm("Are you sure you want to cancel this appointment?")) return;
    try {
      await axios.post(`${API_BASE}/appointments/cancel/${appId}`);
      alert('Appointment Cancelled.');
      fetchMyAppointments();
      fetchDoctors();
    } catch (err) { alert('Cancellation failed.'); }
  };

  const downloadReceipt = (app: any) => {
    const printWindow = window.open('', '_blank');
    if (!printWindow) return;
    printWindow.document.write(`
      <html>
        <head><title>Appointment Receipt - MediSlot</title></head>
        <body style="font-family: Arial, sans-serif; padding: 40px; color: #333;">
          <h2 style="color: #4f46e5;">MediSlot Enterprise - Appointment Slip</h2>
          <hr/>
          <p><strong>Patient Name:</strong> ${userName}</p>
          <p><strong>Doctor:</strong> ${app.timeSlot?.doctor?.name || 'Doctor'} (${app.timeSlot?.doctor?.specialty || 'General'})</p>
          <p><strong>Time Slot:</strong> ${formatSlotTime(app.timeSlot.startTime)} - ${formatSlotTime(app.timeSlot.endTime)}</p>
          <p><strong>Symptoms Notes:</strong> ${app.symptoms || 'None'}</p>
          <p><strong>Status:</strong> <span style="color: green;">Confirmed</span></p>
          <hr/>
          <p style="font-size: 12px; color: #777;">Thank you for choosing MediSlot Enterprise Healthcare.</p>
          <script>window.print();</script>
        </body>
      </html>
    `);
    printWindow.document.close();
  };

  const formatTime = (s: number) => `${Math.floor(s / 60)}:${s % 60 < 10 ? '0' : ''}${s % 60}`;

  if (!token) {
    return (
      <div className="min-h-screen bg-slate-950 text-slate-100 flex items-center justify-center p-6">
        <div className="bg-slate-900 border border-slate-800 p-8 rounded-3xl w-full max-w-md shadow-2xl">
          <div className="flex items-center space-x-3 mb-6">
            <div className="bg-indigo-600 p-3 rounded-2xl"><Calendar className="w-8 h-8 text-white" /></div>
            <div><h1 className="text-2xl font-bold">MediSlot Enterprise</h1><p className="text-xs text-slate-400">Secure Healthcare Portal</p></div>
          </div>
          {isLoginView ? (
            <form onSubmit={handleLogin} className="space-y-4">
              <div><label className="text-xs text-slate-400 mb-1 block">Email</label><input type="email" value={email} onChange={e => setEmail(e.target.value)} required className="w-full bg-slate-950 border border-slate-800 rounded-xl px-4 py-3 text-sm focus:outline-none focus:border-indigo-500" placeholder="user@medislot.com" /></div>
              <div><label className="text-xs text-slate-400 mb-1 block">Password</label><input type="password" value={password} onChange={e => setPassword(e.target.value)} required className="w-full bg-slate-950 border border-slate-800 rounded-xl px-4 py-3 text-sm focus:outline-none focus:border-indigo-500" placeholder="••••••••" /></div>
              <div>
                <label className="text-xs text-slate-400 mb-1 block">Login As Role</label>
                <select value={loginRole} onChange={e => setLoginRole(e.target.value)} className="w-full bg-slate-950 border border-slate-800 rounded-xl px-4 py-3 text-sm text-slate-200 focus:outline-none focus:border-indigo-500">
                  <option value="Patient">Patient</option>
                  <option value="Doctor">Doctor</option>
                </select>
              </div>
              <button type="submit" className="w-full bg-indigo-600 hover:bg-indigo-500 font-semibold py-3 rounded-xl transition shadow-lg">Secure Login</button>
              <p className="text-center text-xs text-slate-400 mt-4">Don't have an account? <span onClick={() => setIsLoginView(false)} className="text-indigo-400 cursor-pointer underline">Register</span></p>
            </form>
          ) : (
            <form onSubmit={handleRegister} className="space-y-4">
              <div><label className="text-xs text-slate-400 mb-1 block">Full Name</label><input type="text" value={name} onChange={e => setName(e.target.value)} required className="w-full bg-slate-950 border border-slate-800 rounded-xl px-4 py-3 text-sm" /></div>
              <div><label className="text-xs text-slate-400 mb-1 block">Email</label><input type="email" value={email} onChange={e => setEmail(e.target.value)} required className="w-full bg-slate-950 border border-slate-800 rounded-xl px-4 py-3 text-sm" /></div>
              <div><label className="text-xs text-slate-400 mb-1 block">Password</label><input type="password" value={password} onChange={e => setPassword(e.target.value)} required className="w-full bg-slate-950 border border-slate-800 rounded-xl px-4 py-3 text-sm" /></div>
              <button type="submit" className="w-full bg-indigo-600 hover:bg-indigo-500 font-semibold py-3 rounded-xl transition shadow-lg">Register</button>
              <p className="text-center text-xs text-slate-400 mt-4">Already registered? <span onClick={() => setIsLoginView(true)} className="text-indigo-400 cursor-pointer underline">Login</span></p>
            </form>
          )}
        </div>
      </div>
    );
  }

  return (
    <div className="min-h-screen bg-slate-950 text-slate-100 font-sans p-6 md:p-10">
      <div className="max-w-6xl mx-auto">
        <header className="flex items-center justify-between border-b border-slate-800 pb-6 mb-8">
          <div className="flex items-center space-x-3">
            <div className="bg-indigo-600 p-2.5 rounded-xl"><Calendar className="w-7 h-7 text-white" /></div>
            <div>
              <h1 className="text-2xl font-extrabold tracking-tight">MediSlot Enterprise</h1>
              <p className="text-sm text-slate-400">Welcome, <span className="text-indigo-400 font-semibold">{userName}</span> ({userRole})</p>
            </div>
          </div>
          <button onClick={handleLogout} className="bg-slate-900 border border-slate-800 hover:bg-slate-800 p-2.5 rounded-xl text-slate-300 transition flex items-center gap-2 text-sm font-medium">
            <LogOut className="w-4 h-4 text-red-400" /> Logout
          </button>
        </header>

        {/* Navigation Tabs */}
        <div className="flex space-x-3 mb-8">
          {userRole === 'Patient' ? (
            <>
              <button onClick={() => setActiveTab('book')} className={`px-5 py-2.5 rounded-xl text-sm font-semibold transition ${activeTab === 'book' ? 'bg-indigo-600 text-white' : 'bg-slate-900 text-slate-400 border border-slate-800'}`}>Book Appointment</button>
              <button onClick={() => { setActiveTab('my-appointments'); fetchMyAppointments(); }} className={`px-5 py-2.5 rounded-xl text-sm font-semibold transition ${activeTab === 'my-appointments' ? 'bg-indigo-600 text-white' : 'bg-slate-900 text-slate-400 border border-slate-800'}`}>My Appointments</button>
            </>
          ) : (
            <button onClick={() => { setActiveTab('doctor-dashboard'); fetchDoctorAppointments(); }} className="px-5 py-2.5 rounded-xl text-sm font-semibold bg-indigo-600 text-white">Doctor Dashboard</button>
          )}
        </div>

        {/* Active Countdown Banner */}
        {selectedSlotId && timeLeft !== null && timeLeft > 0 && (
          <div className="bg-amber-950/40 border border-amber-600/40 p-4 rounded-2xl mb-8 flex items-center justify-between">
            <div className="flex items-center space-x-3"><Clock className="w-6 h-6 text-amber-400 animate-pulse" /><div><p className="font-semibold text-amber-200">Slot Locked in Atomic State</p></div></div>
            <div className="text-2xl font-mono font-bold text-amber-300">{formatTime(timeLeft)}</div>
          </div>
        )}

        {/* Tab 1: Book Appointment */}
        {activeTab === 'book' && userRole === 'Patient' && (
          <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
            {doctors.map((doctor) => (
              <div key={doctor.id} className="bg-slate-900/80 border border-slate-800 rounded-2xl p-6 shadow-xl">
                <h2 className="text-xl font-bold text-white mb-1">{doctor.name}</h2>
                <span className="inline-block bg-indigo-500/10 text-indigo-400 text-xs px-2.5 py-1 rounded-full mb-4">{doctor.specialty}</span>
                <div className="grid grid-cols-2 gap-2.5">
                  {doctor.timeSlots.map((slot: any) => {
                    const isSelected = selectedSlotId === slot.id;
                    return (
                      <div key={slot.id} className={`p-3 rounded-xl border flex flex-col justify-between text-sm ${slot.isBooked ? 'bg-slate-950/40 opacity-50' : 'bg-slate-800/40'}`}>
                        <div className="font-medium text-xs mb-2">{formatSlotTime(slot.startTime)} - {formatSlotTime(slot.endTime)}</div>
                        {slot.isBooked ? <span className="text-xs text-red-400 text-center">Booked</span> :
                         slot.isLocked && !isSelected ? <span className="text-xs text-amber-400 text-center">Locked</span> :
                         <button onClick={() => handleLockSlot(slot.id)} className="w-full bg-indigo-600 hover:bg-indigo-500 text-white text-xs py-1.5 rounded-lg">Lock & Book</button>}
                      </div>
                    );
                  })}
                </div>
              </div>
            ))}
          </div>
        )}

        {/* Tab 2: My Appointments */}
        {activeTab === 'my-appointments' && userRole === 'Patient' && (
          <div className="space-y-4">
            <h2 className="text-xl font-bold mb-4">My Booked Appointments</h2>
            {myAppointments.length === 0 ? <p className="text-slate-400">No appointments found.</p> :
              myAppointments.map(app => (
                <div key={app.id} className="bg-slate-900 border border-slate-800 p-5 rounded-2xl flex items-center justify-between">
                  <div>
                    <h3 className="font-bold text-lg">{app.timeSlot?.doctor?.name}</h3>
                    <p className="text-xs text-indigo-400">{app.timeSlot?.doctor?.specialty}</p>
                    <p className="text-sm text-slate-300 mt-1">Time: {app.timeSlot?.startTime ? formatSlotTime(app.timeSlot.startTime) : ''} - {app.timeSlot?.endTime ? formatSlotTime(app.timeSlot.endTime) : ''}</p>
                    <p className="text-xs text-slate-400 mt-1">Symptoms: {app.symptoms || 'None'}</p>
                    <span className={`inline-block mt-2 text-xs px-2 py-0.5 rounded ${app.status === 'Confirmed' ? 'bg-emerald-950 text-emerald-400' : 'bg-red-950 text-red-400'}`}>{app.status}</span>
                  </div>
                  <div className="flex gap-2">
                    {app.status === 'Confirmed' && (
                      <>
                        <button onClick={() => downloadReceipt(app)} className="bg-slate-800 hover:bg-slate-700 p-2.5 rounded-xl text-xs flex items-center gap-1"><FileText className="w-4 h-4"/> Slip</button>
                        <button onClick={() => handleCancelAppointment(app.id)} className="bg-red-950/60 border border-red-800 hover:bg-red-900 text-red-300 p-2.5 rounded-xl text-xs">Cancel</button>
                      </>
                    )}
                  </div>
                </div>
              ))
            }
          </div>
        )}

        {/* Tab 3: Doctor Dashboard */}
        {activeTab === 'doctor-dashboard' && userRole === 'Doctor' && (
          <div>
            <h2 className="text-xl font-bold mb-4">Patient Appointments & Symptoms Feed</h2>
            {doctorAppointments.length === 0 ? <p className="text-slate-400">No active patient bookings.</p> :
              doctorAppointments.map(app => (
                <div key={app.id} className="bg-slate-900 border border-slate-800 p-5 rounded-2xl mb-4">
                  <div className="flex justify-between items-center mb-2">
                    <span className="font-bold text-indigo-400">Patient ID: #{app.patientId}</span>
                    <span className="text-xs bg-slate-800 px-3 py-1 rounded-full">{app.timeSlot?.startTime ? formatSlotTime(app.timeSlot.startTime) : ''} - {app.timeSlot?.endTime ? formatSlotTime(app.timeSlot.endTime) : ''}</span>
                  </div>
                  <p className="text-sm text-slate-200"><strong>Reported Symptoms:</strong> {app.symptoms || 'No symptoms specified.'}</p>
                </div>
              ))
            }
          </div>
        )}

        {/* Modal */}
        {isModalOpen && (
          <div className="fixed inset-0 bg-black/70 flex items-center justify-center p-4 z-50">
            <div className="bg-slate-900 border border-slate-800 rounded-3xl p-6 max-w-md w-full">
              <h3 className="text-lg font-bold mb-2">Confirm Slot & Symptoms</h3>
              <textarea rows={3} value={symptoms} onChange={e => setSymptoms(e.target.value)} placeholder="Describe your symptoms..." className="w-full bg-slate-950 border border-slate-800 rounded-xl p-3 text-sm text-slate-200 mb-4" />
              <div className="flex gap-3">
                <button onClick={() => setIsModalOpen(false)} className="flex-1 bg-slate-800 py-2.5 rounded-xl text-sm">Cancel</button>
                <button onClick={handleConfirmBooking} className="flex-1 bg-emerald-600 py-2.5 rounded-xl text-sm font-semibold">Confirm Booking</button>
              </div>
            </div>
          </div>
        )}
      </div>
    </div>
  );
}