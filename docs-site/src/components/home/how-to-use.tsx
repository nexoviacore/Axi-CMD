"use client";

import {
  AnimatePresence,
  LayoutGroup,
  motion,
  useReducedMotion,
} from "motion/react";
import { useEffect, useState } from "react";

const AUTO_MS = 7000;

type Tab = { id: string; title: string; summary: string; detail: string };

const TABS: Tab[] = [
  {
    id: "search",
    title: "Search",
    summary:
      "Press Ctrl and K from any page, or use the Search box above the sidebar.",
    detail:
      "The search covers every page and section of this site and runs in your browser, with no server behind it. Results name the page and the section, so you can jump straight to it.",
  },
  {
    id: "move",
    title: "Move around",
    summary:
      'The sidebar lists the pages in reading order. "On this page" lists the sections of the page you are on.',
    detail:
      "Read the sidebar from top to bottom for the full story, or jump to one page. The list on the right follows your scrolling and links to each section, so you always know where you are.",
  },
  {
    id: "read",
    title: "Read the notes",
    summary:
      "Blue notes give context. Amber warnings mark things that go wrong easily.",
    detail:
      "Every page names the file it describes, and the code is the source of truth. If a page and the code disagree, the code wins. Code blocks have a copy button.",
  },
  {
    id: "copy",
    title: "Copy a page",
    summary: "Copy Markdown on each page copies the whole page as Markdown.",
    detail:
      "Paste it into a ticket, a note or an AI assistant. For tools that read the whole site, /llms.txt lists the pages and /llms-full.txt has all of them in one file.",
  },
];

export function HowToUse() {
  const reduce = useReducedMotion();
  const [active, setActive] = useState(0);
  const [paused, setPaused] = useState(false);
  const [picked, setPicked] = useState(false);

  // The tabs advance by themselves until the reader picks one or points at the section.
  const auto = !reduce && !paused && !picked;

  return (
    <div
      className="grid gap-8 lg:grid-cols-[minmax(0,0.9fr)_minmax(0,1.1fr)] lg:gap-12"
      onMouseEnter={() => setPaused(true)}
      onMouseLeave={() => setPaused(false)}
    >
      <div
        role="tablist"
        aria-label="How to use this site"
        className="flex flex-col"
      >
        {TABS.map((tab, i) => {
          const isActive = i === active;
          return (
            <button
              key={tab.id}
              type="button"
              role="tab"
              id={`howto-tab-${tab.id}`}
              aria-selected={isActive}
              aria-controls="howto-panel"
              onClick={() => {
                setActive(i);
                setPicked(true);
              }}
              className="relative border-b border-[var(--ax-line)] py-5 pl-5 text-left outline-none focus-visible:bg-[var(--ax-lav)]"
            >
              <span className="absolute bottom-0 left-0 top-0 w-[3px] bg-[var(--ax-line)]">
                {isActive && (
                  <motion.span
                    key={`${i}-${auto}`}
                    className="block w-full origin-top bg-[var(--ax-indigo)]"
                    initial={{ height: auto ? "0%" : "100%" }}
                    animate={{ height: "100%" }}
                    transition={
                      auto
                        ? { duration: AUTO_MS / 1000, ease: "linear" }
                        : { duration: 0.25 }
                    }
                    onAnimationComplete={() => {
                      if (auto) setActive((a) => (a + 1) % TABS.length);
                    }}
                  />
                )}
              </span>
              <span
                className={`block font-[family-name:var(--font-display)] text-lg font-medium transition-colors ${
                  isActive ? "" : "text-fd-muted-foreground"
                }`}
              >
                {tab.title}
              </span>
              <AnimatePresence initial={false}>
                {isActive && (
                  <motion.span
                    initial={{ height: 0, opacity: 0 }}
                    animate={{ height: "auto", opacity: 1 }}
                    exit={{ height: 0, opacity: 0 }}
                    transition={{ duration: 0.3, ease: [0.22, 1, 0.36, 1] }}
                    className="block overflow-hidden"
                  >
                    <span className="mt-2 block max-w-md text-[0.95rem] leading-relaxed">
                      {tab.summary}
                    </span>
                    <span className="mt-2 block max-w-md text-sm leading-relaxed text-fd-muted-foreground">
                      {tab.detail}
                    </span>
                  </motion.span>
                )}
              </AnimatePresence>
            </button>
          );
        })}
      </div>

      <div
        id="howto-panel"
        role="tabpanel"
        aria-labelledby={`howto-tab-${TABS[active].id}`}
        className="relative h-[24rem] overflow-hidden rounded-2xl border border-[var(--ax-line)] bg-fd-card shadow-[0_24px_60px_-34px_rgba(29,27,58,0.4)]"
      >
        <AnimatePresence mode="wait">
          <motion.div
            key={TABS[active].id}
            initial={reduce ? false : { opacity: 0, y: 12 }}
            animate={{ opacity: 1, y: 0 }}
            exit={reduce ? undefined : { opacity: 0, y: -8 }}
            transition={{ duration: 0.28, ease: "easeOut" }}
            className="absolute inset-0"
          >
            {active === 0 && <SearchVisual reduce={!!reduce} />}
            {active === 1 && <MoveVisual reduce={!!reduce} />}
            {active === 2 && <ReadVisual reduce={!!reduce} />}
            {active === 3 && <CopyVisual reduce={!!reduce} />}
          </motion.div>
        </AnimatePresence>
        <p className="absolute bottom-3 right-4 text-xs text-fd-muted-foreground">
          Illustration of this site
        </p>
      </div>
    </div>
  );
}

const wait = (ms: number) =>
  new Promise<void>((resolve) => setTimeout(resolve, ms));

/* ---------- 1. Search ---------- */

const QUERY = "view filter";
const RESULTS = [
  { page: "View filters", section: "Where the filter is stored" },
  { page: "Database", section: "fn_axi_getstructs_obj" },
  { page: "Glossary", section: "AxiCMD" },
];

function SearchVisual({ reduce }: { reduce: boolean }) {
  const [typed, setTyped] = useState(reduce ? QUERY : "");

  useEffect(() => {
    if (reduce) {
      setTyped(QUERY);
      return;
    }
    let off = false;
    (async () => {
      while (!off) {
        setTyped("");
        await wait(700);
        for (let i = 1; i <= QUERY.length; i++) {
          if (off) return;
          setTyped(QUERY.slice(0, i));
          await wait(85);
        }
        await wait(4200);
      }
    })();
    return () => {
      off = true;
    };
  }, [reduce]);

  return (
    <div className="flex h-full flex-col items-center px-5 pt-8">
      <div className="mb-3 flex items-center gap-1.5 self-end text-xs text-fd-muted-foreground">
        <span>Open from anywhere</span>
        <motion.kbd
          className="rounded border border-fd-border bg-fd-muted px-1.5 py-0.5 font-mono"
          animate={reduce ? undefined : { y: [0, 2, 0] }}
          transition={{ duration: 0.5, repeat: Infinity, repeatDelay: 4.2 }}
        >
          Ctrl
        </motion.kbd>
        <motion.kbd
          className="rounded border border-fd-border bg-fd-muted px-1.5 py-0.5 font-mono"
          animate={reduce ? undefined : { y: [0, 2, 0] }}
          transition={{
            duration: 0.5,
            repeat: Infinity,
            repeatDelay: 4.2,
            delay: 0.08,
          }}
        >
          K
        </motion.kbd>
      </div>
      <div className="w-full max-w-md overflow-hidden rounded-xl border border-fd-border bg-fd-background shadow-lg">
        <div className="flex items-center gap-2 border-b border-fd-border px-4 py-3 text-sm">
          <svg
            width="15"
            height="15"
            viewBox="0 0 24 24"
            fill="none"
            stroke="currentColor"
            strokeWidth="2"
            className="text-fd-muted-foreground"
            aria-hidden
          >
            <circle cx="11" cy="11" r="7" />
            <path d="m20 20-3.5-3.5" />
          </svg>
          <span>{typed}</span>
          <motion.span
            className="h-4 w-[2px] bg-[var(--ax-indigo)]"
            animate={reduce ? undefined : { opacity: [1, 0, 1] }}
            transition={{ duration: 1, repeat: Infinity }}
          />
        </div>
        <ul className="min-h-[10.5rem] p-1.5">
          <AnimatePresence>
            {typed.length >= 4 &&
              RESULTS.map((r, i) => (
                <motion.li
                  key={r.page}
                  initial={{ opacity: 0, x: -8 }}
                  animate={{ opacity: 1, x: 0 }}
                  exit={{ opacity: 0 }}
                  transition={{ delay: i * 0.07, duration: 0.22 }}
                  className={`rounded-lg px-3 py-2.5 text-sm ${i === 0 ? "bg-[var(--ax-lav)]" : ""}`}
                >
                  <span className="block font-medium">{r.page}</span>
                  <span className="block text-xs text-fd-muted-foreground">
                    {r.section}
                  </span>
                </motion.li>
              ))}
          </AnimatePresence>
        </ul>
      </div>
    </div>
  );
}

/* ---------- 2. Move around ---------- */

const SIDEBAR = [
  "Introduction",
  "Architecture",
  "Commands",
  "Data loading",
  "View filters",
  "Database",
  "AxiApi",
];
const TOC = [
  "Where the filter is stored",
  "What the function does",
  "Sending the session variables",
  "Testing it",
  "History",
];

function MoveVisual({ reduce }: { reduce: boolean }) {
  const [side, setSide] = useState(reduce ? 4 : 1);
  const [toc, setToc] = useState(0);

  useEffect(() => {
    if (reduce) return;
    const a = setInterval(() => setSide((s) => (s >= 5 ? 1 : s + 1)), 1700);
    const b = setInterval(() => setToc((t) => (t + 1) % TOC.length), 1100);
    return () => {
      clearInterval(a);
      clearInterval(b);
    };
  }, [reduce]);

  return (
    <div className="grid h-full grid-cols-[9.5rem_1fr_8.5rem] gap-3 p-4 pb-9 text-sm sm:grid-cols-[11rem_1fr_11rem]">
      <LayoutGroup id="sidebar">
        <ul className="flex flex-col gap-0.5">
          {SIDEBAR.map((item, i) => (
            <li
              key={item}
              className="relative rounded-md px-2.5 py-1.5 text-[0.8rem]"
            >
              {i === side && (
                <motion.span
                  layoutId="side-pill"
                  className="absolute inset-0 rounded-md bg-[var(--ax-lav)]"
                  transition={{ type: "spring", stiffness: 420, damping: 34 }}
                />
              )}
              <span
                className={`relative ${i === side ? "font-medium" : "text-fd-muted-foreground"}`}
              >
                {item}
              </span>
            </li>
          ))}
        </ul>
      </LayoutGroup>

      <div className="flex flex-col gap-2.5 pt-1" aria-hidden>
        <div className="h-5 w-2/3 rounded bg-fd-muted" />
        <div className="h-2.5 w-full rounded bg-fd-muted/70" />
        <div className="h-2.5 w-11/12 rounded bg-fd-muted/70" />
        <div className="mt-3 h-4 w-1/2 rounded bg-fd-muted" />
        <div className="h-2.5 w-full rounded bg-fd-muted/70" />
        <div className="h-2.5 w-4/5 rounded bg-fd-muted/70" />
        <div className="h-16 w-full rounded-lg border border-fd-border bg-fd-muted/40" />
      </div>

      <div>
        <p className="mb-2 text-xs text-fd-muted-foreground">On this page</p>
        <LayoutGroup id="toc">
          <ul className="relative flex flex-col gap-0.5 border-l border-fd-border">
            {TOC.map((item, i) => (
              <li
                key={item}
                className="relative py-1 pl-3 text-[0.75rem] leading-snug"
              >
                {i === toc && (
                  <motion.span
                    layoutId="toc-marker"
                    className="absolute -left-px bottom-0 top-0 w-[2px] bg-[var(--ax-indigo)]"
                    transition={{ type: "spring", stiffness: 420, damping: 34 }}
                  />
                )}
                <span
                  className={
                    i === toc ? "font-medium" : "text-fd-muted-foreground"
                  }
                >
                  {item}
                </span>
              </li>
            ))}
          </ul>
        </LayoutGroup>
      </div>
    </div>
  );
}

/* ---------- 3. Read the notes ---------- */

function ReadVisual({ reduce }: { reduce: boolean }) {
  const [copied, setCopied] = useState(false);

  useEffect(() => {
    if (reduce) return;
    const t = setInterval(() => setCopied((c) => !c), 2200);
    return () => clearInterval(t);
  }, [reduce]);

  return (
    <div className="flex h-full flex-col justify-center gap-3 px-5 pb-8 pt-4 text-sm">
      <motion.div
        initial={reduce ? false : { opacity: 0, x: -12 }}
        animate={{ opacity: 1, x: 0 }}
        transition={{ delay: 0.1, duration: 0.35 }}
        className="rounded-lg border border-sky-500/30 bg-sky-500/10 px-4 py-3"
      >
        <p className="font-medium">Source of truth</p>
        <p className="mt-0.5 text-[0.8rem] text-fd-muted-foreground">
          When a page and the code disagree, the code wins.
        </p>
      </motion.div>
      <motion.div
        initial={reduce ? false : { opacity: 0, x: -12 }}
        animate={{ opacity: 1, x: 0 }}
        transition={{ delay: 0.25, duration: 0.35 }}
        className="rounded-lg border border-amber-500/40 bg-amber-500/10 px-4 py-3"
      >
        <p className="font-medium">Skipped filter means a full list</p>
        <p className="mt-0.5 text-[0.8rem] text-fd-muted-foreground">
          A missing session variable shows more records, not fewer.
        </p>
      </motion.div>
      <motion.div
        initial={reduce ? false : { opacity: 0, x: -12 }}
        animate={{ opacity: 1, x: 0 }}
        transition={{ delay: 0.4, duration: 0.35 }}
        className="flex items-center justify-between rounded-lg border border-fd-border bg-fd-background px-4 py-3"
      >
        <code className="truncate font-mono text-[0.78rem]">
          node --check axicmdmain.js
        </code>
        <span className="ml-3 flex h-6 min-w-[3.6rem] items-center justify-center rounded-md border border-fd-border px-2 text-[0.7rem] text-fd-muted-foreground">
          <AnimatePresence mode="wait" initial={false}>
            <motion.span
              key={copied ? "done" : "copy"}
              initial={{ opacity: 0, y: 4 }}
              animate={{ opacity: 1, y: 0 }}
              exit={{ opacity: 0, y: -4 }}
              transition={{ duration: 0.15 }}
            >
              {copied ? "Copied" : "Copy"}
            </motion.span>
          </AnimatePresence>
        </span>
      </motion.div>
    </div>
  );
}

/* ---------- 4. Copy a page ---------- */

function CopyVisual({ reduce }: { reduce: boolean }) {
  const [copied, setCopied] = useState(false);
  const [pulse, setPulse] = useState(0);

  useEffect(() => {
    if (reduce) return;
    let off = false;
    (async () => {
      while (!off) {
        await wait(1500);
        if (off) return;
        setCopied(true);
        setPulse((p) => p + 1);
        await wait(2000);
        if (off) return;
        setCopied(false);
      }
    })();
    return () => {
      off = true;
    };
  }, [reduce]);

  return (
    <div className="flex h-full flex-col gap-5 px-6 pb-9 pt-7">
      <div>
        <p className="font-[family-name:var(--font-display)] text-2xl font-semibold">
          View filters
        </p>
        <div className="mt-3 flex items-center gap-2">
          <span className="relative">
            <span className="relative z-10 flex items-center gap-1.5 rounded-md border border-fd-border bg-fd-background px-2.5 py-1.5 text-xs font-medium">
              <AnimatePresence mode="wait" initial={false}>
                <motion.span
                  key={copied ? "done" : "copy"}
                  initial={{ opacity: 0, y: 4 }}
                  animate={{ opacity: 1, y: 0 }}
                  exit={{ opacity: 0, y: -4 }}
                  transition={{ duration: 0.15 }}
                >
                  {copied ? "Copied" : "Copy Markdown"}
                </motion.span>
              </AnimatePresence>
            </span>
            {pulse > 0 && (
              <motion.span
                key={pulse}
                aria-hidden
                className="absolute inset-0 rounded-md border-2 border-[var(--ax-indigo)]"
                initial={{ opacity: 0.8, scale: 1 }}
                animate={{ opacity: 0, scale: 1.25 }}
                transition={{ duration: 0.7 }}
              />
            )}
          </span>
          <span className="rounded-md border border-fd-border bg-fd-background px-2.5 py-1.5 text-xs font-medium">
            Open
          </span>
        </div>
      </div>

      <motion.div
        aria-hidden
        className="rounded-lg border border-dashed border-fd-border bg-fd-muted/40 p-4 font-mono text-[0.72rem] leading-relaxed text-fd-muted-foreground"
        animate={{ borderColor: copied ? "var(--ax-indigo)" : undefined }}
      >
        <p># View filters</p>
        <p>An Axpert form can have a view filter...</p>
        <p>## Where the filter is stored</p>
      </motion.div>

      <div className="grid grid-cols-2 gap-3 text-xs">
        <div className="rounded-lg border border-fd-border bg-fd-background p-3">
          <p className="font-mono font-medium">/llms.txt</p>
          <p className="mt-1 text-fd-muted-foreground">
            An index of every page.
          </p>
        </div>
        <div className="rounded-lg border border-fd-border bg-fd-background p-3">
          <p className="font-mono font-medium">/llms-full.txt</p>
          <p className="mt-1 text-fd-muted-foreground">
            All pages in one file.
          </p>
        </div>
      </div>
    </div>
  );
}
