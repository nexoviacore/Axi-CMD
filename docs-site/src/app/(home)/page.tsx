import Image from "next/image";
import Link from "next/link";
import { EntryLink } from "@/components/home/entry-link";
import { HowToUse } from "@/components/home/how-to-use";
import { PaletteDemo } from "@/components/home/palette-demo";
import { ReadingPaths } from "@/components/home/reading-paths";
import { repoUrl } from "@/lib/shared";
import { Reveal } from "@/components/home/reveal";

const ENTRIES = [
  {
    href: "/docs/architecture",
    title: "Architecture",
    text: "The parts of AxiCMD and how one request travels from the bar to the database and back.",
  },
  {
    href: "/docs/data-loading",
    title: "Data loading",
    text: "Paging, search and the three cache layers behind every list.",
  },
  {
    href: "/docs/view-filters",
    title: "View filters",
    text: "How a form’s filter limits the records that view and edit show.",
  },
  {
    href: "/docs/database",
    title: "Database",
    text: "Tables, the data source row and the SQL functions, for Postgres and Oracle.",
  },
  {
    href: "/docs/axiapi",
    title: "AxiApi",
    text: "Endpoints for command configuration, favourites and settings.",
  },
  {
    href: "/docs/operations",
    title: "Operations",
    text: "Releasing a change, clearing caches and fixing the usual problems.",
  },
];

const FOOTER_DOCS = [
  { href: "/docs", label: "Introduction" },
  { href: "/docs/architecture", label: "Architecture" },
  { href: "/docs/commands", label: "Commands" },
  { href: "/docs/data-loading", label: "Data loading" },
  { href: "/docs/view-filters", label: "View filters" },
];

const FOOTER_MORE = [
  { href: "/docs/database", label: "Database" },
  { href: "/docs/axiapi", label: "AxiApi" },
  { href: "/docs/operations", label: "Operations" },
  { href: "/docs/contributing", label: "Contributing" },
  { href: "/docs/glossary", label: "Glossary" },
];

const footerLink =
  "text-sm text-fd-muted-foreground transition-colors hover:text-[var(--ax-indigo)] focus-visible:outline focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-[var(--ax-indigo)]";

export default function HomePage() {
  return (
    <>
      <main className="axi-home flex-1 overflow-x-clip">
        <section className="mx-auto grid w-full max-w-6xl items-center gap-12 px-6 pb-16 pt-14 lg:grid-cols-[1fr_1.1fr] lg:pb-24 lg:pt-24">
          <div className="text-left">
            <Reveal step={0}>
              <h1 className="font-[family-name:var(--font-display)] text-5xl font-semibold leading-[1.05] tracking-tight sm:text-6xl">
                AxiCMD
              </h1>
            </Reveal>
            <Reveal step={1}>
              <p className="mt-5 max-w-md text-xl leading-snug text-[var(--ax-ink)]">
                Type a command, pick a result, and Axpert opens it.
              </p>
            </Reveal>
            <Reveal step={2}>
              <p className="mt-4 max-w-md leading-relaxed text-fd-muted-foreground">
                These docs explain how the command palette finds, filters and
                loads what you see, for the people who build and maintain it.
              </p>
            </Reveal>
            <Reveal step={3} className="mt-8 flex flex-wrap items-center gap-3">
              <Link
                href="/docs"
                className="rounded-lg bg-[var(--ax-indigo)] px-5 py-2.5 text-sm font-medium text-white shadow-sm transition-transform hover:-translate-y-0.5 focus-visible:outline focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-[var(--ax-indigo)]"
              >
                Read the docs
              </Link>
              <Link
                href="/docs/architecture"
                className="rounded-lg border border-[var(--ax-line)] px-5 py-2.5 text-sm font-medium transition-colors hover:bg-[var(--ax-lav)] focus-visible:outline focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-[var(--ax-indigo)]"
              >
                See how a request flows
              </Link>
            </Reveal>
          </div>

          <Reveal step={4}>
            <PaletteDemo />
          </Reveal>
        </section>

        <section className="mx-auto w-full max-w-6xl px-6 pb-16 lg:pb-24">
          <figure>
            <Image
              src="/images/axicmd-palette.png"
              alt="The AxiCMD command palette open in the Axpert Web shell, showing the Commands row and a list of structures"
              width={1881}
              height={780}
              className="h-auto w-full rounded-2xl border border-[var(--ax-line)] shadow-[0_24px_60px_-28px_rgba(29,27,58,0.35)]"
            />
            <figcaption className="mt-3 max-w-2xl text-sm leading-relaxed text-fd-muted-foreground">
              The palette in the Axpert Web shell: commands at the top, matching
              structures below, and the command bar docked at the bottom.
            </figcaption>
          </figure>
        </section>

        <section className="mx-auto w-full max-w-6xl px-6 pb-24">
          <h2 className="font-[family-name:var(--font-display)] text-2xl font-semibold tracking-tight">
            Where to start
          </h2>
          <div className="mt-4 grid gap-x-12 md:grid-cols-2">
            {ENTRIES.map((e) => (
              <EntryLink key={e.href} href={e.href} title={e.title}>
                {e.text}
              </EntryLink>
            ))}
          </div>
        </section>

        <section className="border-t border-[var(--ax-line)] bg-[var(--ax-lav)]/40">
          <div className="mx-auto w-full max-w-6xl px-6 py-20">
            <h2 className="font-[family-name:var(--font-display)] text-3xl font-semibold tracking-tight">
              How to use this site
            </h2>
            <p className="mt-3 max-w-2xl leading-relaxed text-fd-muted-foreground">
              Four things make these docs quick to read. Pick one on the left to
              see it, or let the examples play.
            </p>
            <div className="mt-10">
              <HowToUse />
            </div>

            <h3 className="mt-20 font-[family-name:var(--font-display)] text-2xl font-semibold tracking-tight">
              Suggested reading routes
            </h3>
            <p className="mt-2 max-w-2xl leading-relaxed text-fd-muted-foreground">
              You do not have to read everything in order. Start from what you
              are trying to do.
            </p>
            <ReadingPaths />
          </div>
        </section>
      </main>
      <footer className="axi-home border-t border-[var(--ax-line)]">
        <div className="mx-auto grid w-full max-w-6xl gap-10 px-6 py-12 md:grid-cols-[1.4fr_1fr_1fr]">
          <div>
            <p className="font-[family-name:var(--font-display)] text-xl font-semibold">
              AxiCMD Docs
            </p>
            <p className="mt-3 max-w-xs text-sm leading-relaxed text-fd-muted-foreground">
              Developer documentation for the Axpert Web command palette: how it
              finds, filters and loads what you see.
            </p>
          </div>
          <nav aria-label="Documentation">
            <p className="text-sm font-medium">Read</p>
            <ul className="mt-3 flex flex-col gap-2">
              {FOOTER_DOCS.map((l) => (
                <li key={l.href}>
                  <Link href={l.href} className={footerLink}>
                    {l.label}
                  </Link>
                </li>
              ))}
            </ul>
          </nav>
          <nav aria-label="More documentation">
            <p className="text-sm font-medium">Build and maintain</p>
            <ul className="mt-3 flex flex-col gap-2">
              {FOOTER_MORE.map((l) => (
                <li key={l.href}>
                  <Link href={l.href} className={footerLink}>
                    {l.label}
                  </Link>
                </li>
              ))}
              <li>
                <a href="/llms.txt" className={footerLink}>
                  llms.txt
                </a>
              </li>
              <li>
                <a
                  href={repoUrl}
                  className={footerLink}
                  target="_blank"
                  rel="noreferrer"
                >
                  GitHub repository
                </a>
              </li>
            </ul>
          </nav>
        </div>
        <div className="border-t border-[var(--ax-line)]">
          <p className="mx-auto w-full max-w-6xl px-6 py-5 text-xs text-fd-muted-foreground">
            Built with Next.js and Fumadocs. When a page and the code disagree,
            the code wins.
          </p>
        </div>
      </footer>
    </>
  );
}
