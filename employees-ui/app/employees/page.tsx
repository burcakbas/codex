'use client';
import Link from 'next/link';
import { useEffect, useState } from 'react';

interface Employee {
  id: number;
  ssn: string;
  firstName: string;
  lastName: string;
  salary: number;
}

export default function EmployeesPage() {
  const [employees, setEmployees] = useState<Employee[]>([]);
  const [sortBy, setSortBy] = useState('LastName');
  const [dir, setDir] = useState<'asc' | 'desc'>('asc');
  const [q, setQ] = useState('');
  const [search, setSearch] = useState('');

  useEffect(() => {
    const t = setTimeout(() => setSearch(q), 300);
    return () => clearTimeout(t);
  }, [q]);

  useEffect(() => {
    fetch(`${process.env.NEXT_PUBLIC_API_BASE_URL}/api/employees?sortBy=${sortBy}&dir=${dir}&q=${encodeURIComponent(search)}`)
      .then(r => r.json())
      .then(setEmployees)
      .catch(() => setEmployees([]));
  }, [sortBy, dir, search]);

  const toggleSort = (field: string) => {
    if (sortBy === field) {
      setDir(dir === 'asc' ? 'desc' : 'asc');
    } else {
      setSortBy(field);
      setDir('asc');
    }
  };

  return (
    <div>
      <div className="flex justify-between mb-4">
        <input value={q} onChange={e => setQ(e.target.value)} placeholder="Search..." className="border p-2" />
        <Link href="/employees/new" className="bg-blue-500 text-white px-4 py-2 rounded">New Employee</Link>
      </div>
      <table className="min-w-full table-auto border">
        <thead>
          <tr>
            <th className="border px-2">ID</th>
            <th className="border px-2">SSN</th>
            <th className="border px-2">First</th>
            <th className="border px-2 cursor-pointer" onClick={() => toggleSort('LastName')}>Last</th>
            <th className="border px-2 cursor-pointer" onClick={() => toggleSort('Salary')}>Salary</th>
          </tr>
        </thead>
        <tbody>
          {employees.map(e => (
            <tr key={e.id} className="hover:bg-gray-100">
              <td className="border px-2">{e.id}</td>
              <td className="border px-2">{e.ssn}</td>
              <td className="border px-2">{e.firstName}</td>
              <td className="border px-2">
                <Link href={`/employees/${e.id}`} className="text-blue-600 underline">{e.lastName}</Link>
              </td>
              <td className="border px-2">{e.salary.toFixed(2)}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}
