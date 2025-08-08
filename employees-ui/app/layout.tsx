import './globals.css';
import { Toaster } from 'react-hot-toast';
import type { Metadata } from 'next';

export const metadata: Metadata = {
  title: 'Employees Demo',
};

export default function RootLayout({ children }: { children: React.ReactNode }) {
  return (
    <html lang="en">
      <body className="p-4">
        <Toaster position="top-right" />
        {children}
      </body>
    </html>
  );
}
