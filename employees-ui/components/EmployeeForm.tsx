'use client';
import { useState } from 'react';
import { useRouter } from 'next/navigation';
import { z } from 'zod';
import toast from 'react-hot-toast';

const schema = z.object({
  ssn: z.string().length(11, 'SSN must be 11 characters'),
  firstName: z.string().min(1),
  lastName: z.string().min(1),
  salary: z.number().min(0),
});

type Employee = {
  id?: number;
  ssn: string;
  firstName: string;
  lastName: string;
  salary: number;
};

export default function EmployeeForm({ employee }: { employee?: Employee }) {
  const [form, setForm] = useState<Employee>(employee ?? { ssn: '', firstName: '', lastName: '', salary: 0 });
  const router = useRouter();

  const submit = async (e: React.FormEvent) => {
    e.preventDefault();
    const parsed = schema.safeParse({ ...form, salary: Number(form.salary) });
    if (!parsed.success) {
      toast.error(parsed.error.errors[0].message);
      return;
    }
    const method = employee ? 'PUT' : 'POST';
    const url = employee
      ? `${process.env.NEXT_PUBLIC_API_BASE_URL}/api/employees/${employee.id}`
      : `${process.env.NEXT_PUBLIC_API_BASE_URL}/api/employees`;
    const res = await fetch(url, {
      method,
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(parsed.data),
    });
    if (res.ok) {
      toast.success('Saved');
      router.push('/employees');
    } else {
      toast.error('Error');
    }
  };

  const del = async () => {
    if (!employee) return;
    const res = await fetch(`${process.env.NEXT_PUBLIC_API_BASE_URL}/api/employees/${employee.id}`, { method: 'DELETE' });
    if (res.ok) {
      toast.success('Deleted');
      router.push('/employees');
    } else {
      toast.error('Error');
    }
  };

  return (
    <form onSubmit={submit} className="space-y-4 max-w-md">
      <div>
        <label className="block mb-1">SSN</label>
        <input value={form.ssn} onChange={e => setForm({ ...form, ssn: e.target.value })} className="border p-2 w-full" required />
      </div>
      <div>
        <label className="block mb-1">First Name</label>
        <input value={form.firstName} onChange={e => setForm({ ...form, firstName: e.target.value })} className="border p-2 w-full" required />
      </div>
      <div>
        <label className="block mb-1">Last Name</label>
        <input value={form.lastName} onChange={e => setForm({ ...form, lastName: e.target.value })} className="border p-2 w-full" required />
      </div>
      <div>
        <label className="block mb-1">Salary</label>
        <input type="number" step="0.01" min="0" value={form.salary} onChange={e => setForm({ ...form, salary: parseFloat(e.target.value) })} className="border p-2 w-full" required />
      </div>
      <div className="flex gap-2">
        <button type="submit" className="bg-green-600 text-white px-4 py-2 rounded">{employee ? 'Update' : 'Create'}</button>
        {employee && <button type="button" onClick={del} className="bg-red-600 text-white px-4 py-2 rounded">Delete</button>}
      </div>
    </form>
  );
}
