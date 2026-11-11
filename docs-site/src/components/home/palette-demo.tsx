"use client";

import { AnimatePresence, motion, useReducedMotion } from "motion/react";
import { useEffect, useState } from "react";

type Demo = { text: string; heading: string; rows: string[] };

// Sample commands and results. They are illustrative, not data from a real application.
const DEMOS: Demo[] = [
  {
    text: "cityf view",
    heading: "Records of cityf",
    rows: ["Chennai [CHN]", "Mumbai [MUM]", "Pune [PUN]", "Delhi [DEL]"],
  },
  {
    text: "invoice edit",
    heading: "Records of invoice",
    rows: ["1001 [INV-1001]", "1002 [INV-1002]", "1003 [INV-1003]"],
  },
  {
    text: "tfrgt view",
    heading: "Records of tfrgt",
    rows: [
      "Grand total A [TF-001]",
      "Grand total B [TF-002]",
      "Grand total C [TF-003]",
    ],
  },
  {
    text: "sfvi edit",
    heading: "Records of sfvi",
    rows: [
      "Vanishing field [SF-014]",
      "Script check [SF-015]",
      "Field reload [SF-016]",
    ],
  },
];

const TYPE_MS = 65;
const ERASE_MS = 22;
const HOLD_MS = 2600;

export function PaletteDemo() {
  const reduce = useReducedMotion();
  const [index, setIndex] = useState(0);
  const [typed, setTyped] = useState(reduce ? DEMOS[0].text : "");
  const [showRows, setShowRows] = useState(!!reduce);

  useEffect(() => {
    if (reduce) {
      setIndex(0);
      setTyped(DEMOS[0].text);
      setShowRows(true);
      return;
    }
    let cancelled = false;
    const wait = (ms: number) =>
      new Promise<void>((resolve) => setTimeout(resolve, ms));

    (async () => {
      await wait(900);
      let i = 0;
      while (!cancelled) {
        const demo = DEMOS[i % DEMOS.length];
        setIndex(i % DEMOS.length);
        for (let c = 1; c <= demo.text.length; c++) {
          if (cancelled) return;
          setTyped(demo.text.slice(0, c));
          await wait(TYPE_MS);
        }
        setShowRows(true);
        await wait(HOLD_MS);
        setShowRows(false);
        await wait(250);
        for (let c = demo.text.length; c >= 0; c--) {
          if (cancelled) return;
          setTyped(demo.text.slice(0, c));
          await wait(ERASE_MS);
        }
        await wait(300);
        i++;
      }
    })();

    return () => {
      cancelled = true;
    };
  }, [reduce]);

  const demo = DEMOS[index];

  return (
    <div
      role="img"
      aria-label="Animated example of the AxiCMD command bar. Typing the form name and then a command, such as cityf view, lists the matching records above the bar."
      className="relative h-[22rem] w-full overflow-hidden rounded-2xl border border-[var(--ax-line)] shadow-[0_30px_80px_-30px_rgba(91,78,224,0.45)] sm:h-[24rem]"
      style={{
        background:
          "linear-gradient(160deg, #6a5df0 0%, #5146d8 55%, #3f36b8 100%)",
      }}
    >
      {/* soft light drifting behind the panel, the only ambient motion on the page */}
      <motion.div
        aria-hidden
        className="absolute -right-10 -top-16 h-72 w-72 rounded-full bg-white/20 blur-3xl"
        animate={reduce ? undefined : { x: [0, -36, 0], y: [0, 26, 0] }}
        transition={{ duration: 16, repeat: Infinity, ease: "easeInOut" }}
      />

      {/* faint shapes that hint at the shell behind the bar */}
      <div
        aria-hidden
        className="absolute left-4 right-4 top-4 flex gap-3 sm:left-6 sm:right-6 sm:top-6"
      >
        <div className="h-20 flex-1 rounded-xl bg-white/10" />
        <div className="hidden h-20 flex-1 rounded-xl bg-white/10 sm:block" />
      </div>

      {/* results open upward from the bar, as in the product */}
      <div className="absolute inset-x-4 bottom-[4.25rem] flex min-h-0 flex-col justify-end sm:inset-x-6">
        <AnimatePresence mode="wait">
          {showRows && (
            <motion.div
              key={demo.text}
              initial={{ opacity: 0, y: 14, scale: 0.985 }}
              animate={{ opacity: 1, y: 0, scale: 1 }}
              exit={{ opacity: 0, y: 8 }}
              transition={{ type: "spring", stiffness: 380, damping: 30 }}
              className="overflow-hidden rounded-xl bg-white text-left text-[#1d1b3a] shadow-xl"
            >
              <p className="border-b border-[#1d1b3a]/10 px-4 py-2.5 text-xs font-medium text-[#1d1b3a]/60">
                {demo.heading}
              </p>
              <ul>
                {demo.rows.map((row, i) => (
                  <motion.li
                    key={row}
                    initial={{ opacity: 0, x: -8 }}
                    animate={{ opacity: 1, x: 0 }}
                    transition={{
                      delay: 0.08 + i * 0.07,
                      duration: 0.25,
                      ease: "easeOut",
                    }}
                    className={`px-4 py-2.5 text-sm ${i === 0 ? "bg-[#f1efff]" : ""} ${
                      i > 0 ? "border-t border-[#1d1b3a]/5" : ""
                    }`}
                  >
                    {row}
                  </motion.li>
                ))}
              </ul>
            </motion.div>
          )}
        </AnimatePresence>
      </div>

      {/* the command bar */}
      <div className="absolute inset-x-0 bottom-0 flex h-14 items-center gap-3 bg-white px-4 text-[#1d1b3a] sm:px-6">
        <span className="font-[family-name:var(--font-display)] text-lg font-semibold text-[#e5484d]">
          cmd
        </span>
        <span className="h-6 w-px bg-[#1d1b3a]/15" />
        <span className="flex min-w-0 items-center text-[0.95rem]" aria-hidden>
          <span className="truncate">{typed}</span>
          <motion.span
            className="ml-0.5 inline-block h-5 w-[2px] bg-[#5b4ee0]"
            animate={reduce ? undefined : { opacity: [1, 0, 1] }}
            transition={{ duration: 1, repeat: Infinity }}
          />
          {typed === "" && (
            <span className="ml-1 text-[#1d1b3a]/40">Axpert AI</span>
          )}
        </span>
      </div>
    </div>
  );
}
