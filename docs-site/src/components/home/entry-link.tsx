"use client";

import { motion } from "motion/react";
import Link from "next/link";

/** A row of the "where to start" list. The marker slides in on hover and focus, so the row answers the pointer. */
export function EntryLink({
  href,
  title,
  children,
}: {
  href: string;
  title: string;
  children: React.ReactNode;
}) {
  return (
    <motion.div
      initial="rest"
      whileHover="active"
      whileFocus="active"
      animate="rest"
    >
      <Link
        href={href}
        className="group flex items-start gap-4 border-b border-[var(--ax-line)] py-5 outline-none focus-visible:bg-[var(--ax-lav)]"
      >
        <motion.span
          aria-hidden
          variants={{
            rest: { scaleY: 0.35, opacity: 0.4 },
            active: { scaleY: 1, opacity: 1 },
          }}
          transition={{ type: "spring", stiffness: 500, damping: 30 }}
          className="mt-1 h-6 w-1 origin-center rounded-full bg-[var(--ax-indigo)]"
        />
        <motion.span
          variants={{ rest: { x: 0 }, active: { x: 4 } }}
          transition={{ type: "spring", stiffness: 500, damping: 32 }}
          className="block"
        >
          <span className="block font-[family-name:var(--font-display)] text-lg font-medium">
            {title}
          </span>
          <span className="mt-1 block max-w-md text-sm leading-relaxed text-fd-muted-foreground">
            {children}
          </span>
        </motion.span>
      </Link>
    </motion.div>
  );
}
