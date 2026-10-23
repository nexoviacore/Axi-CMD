import Link from 'next/link';

export default function HomePage() {
  return (
    <div className="flex flex-col justify-center items-center text-center flex-1 gap-4 px-4">
      <h1 className="text-3xl font-bold">AxiCMD Documentation</h1>
      <p className="max-w-xl text-fd-muted-foreground">
        How the Axpert Web command palette works: architecture, data loading, view filters, the database and AxiApi.
      </p>
      <Link
        href="/docs"
        className="rounded-md bg-fd-primary px-4 py-2 text-sm font-medium text-fd-primary-foreground"
      >
        Read the docs
      </Link>
    </div>
  );
}
