"use client";

import { motion, useReducedMotion } from "motion/react";
import Link from "next/link";

type Stop = { label: string; href: string };
type Path = { goal: string; stops: Stop[] };

const PATHS: Path[] = [
  {
    goal: "I am new to AxiCMD",
    stops: [
      { label: "Introduction", href: "/docs" },
      { label: "Architecture", href: "/docs/architecture" },
      { label: "Commands", href: "/docs/commands" },
    ],
  },
  {
    goal: "I am changing how lists load",
    stops: [
      { label: "Data loading", href: "/docs/data-loading" },
      { label: "View filters", href: "/docs/view-filters" },
      { label: "Database", href: "/docs/database" },
    ],
  },
  {
    goal: "Something is not working",
    stops: [
      { label: "Operations", href: "/docs/operations" },
      { label: "Data loading", href: "/docs/data-loading" },
      { label: "Glossary", href: "/docs/glossary" },
    ],
  },
  {
    goal: "I am working on the API",
    stops: [
      { label: "AxiApi", href: "/docs/axiapi" },
      { label: "Database", href: "/docs/database" },
      { label: "Contributing", href: "/docs/contributing" },
    ],
  },
];

/** Each row is a short route through the docs. The line between the stops draws itself when the row comes into view. */
export function ReadingPaths() {
  const reduce = useReducedMotion();

  return (
    <ul className="mt-6 divide-y divide-[var(--ax-line)] border-y border-[var(--ax-line)]">
      {PATHS.map((path, row) => (
        <li
          key={path.goal}
          className="grid items-center gap-3 py-5 md:grid-cols-[14rem_1fr]"
        >
          <p className="font-[family-name:var(--font-display)] text-lg font-medium">
            {path.goal}
          </p>
          <ol className="flex flex-wrap items-center gap-y-3">
            {path.stops.map((stop, i) => (
              <li key={stop.href + stop.label} className="flex items-center">
                {i > 0 && (
                  <motion.span
                    aria-hidden
                    className="mx-2 block h-px w-8 origin-left bg-[var(--ax-indigo)] sm:w-12"
                    initial={reduce ? false : { scaleX: 0 }}
                    whileInView={{ scaleX: 1 }}
                    viewport={{ once: true, margin: "-40px" }}
                    transition={{
                      delay: row * 0.08 + i * 0.25,
                      duration: 0.45,
                      ease: "easeOut",
                    }}
                  />
                )}
                <motion.span
                  initial={reduce ? false : { opacity: 0.35 }}
                  whileInView={{ opacity: 1 }}
                  viewport={{ once: true, margin: "-40px" }}
                  transition={{ delay: row * 0.08 + i * 0.25, duration: 0.4 }}
                >
                  <Link
                    href={stop.href}
                    className="rounded-full border border-[var(--ax-line)] px-3.5 py-1.5 text-sm transition-colors hover:border-[var(--ax-indigo)] hover:bg-[var(--ax-lav)] focus-visible:outline focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-[var(--ax-indigo)]"
                  >
                    {stop.label}
                  </Link>
                </motion.span>
              </li>
            ))}
          </ol>
        </li>
      ))}
    </ul>
  );
}
