"use client";

import { motion, useReducedMotion, type HTMLMotionProps } from "motion/react";

/**
 * Part of the single page-load sequence: children enter one after another, in order of `step`.
 * With reduced motion they simply appear.
 */
export function Reveal({
  step = 0,
  children,
  ...props
}: { step?: number } & HTMLMotionProps<"div">) {
  const reduce = useReducedMotion();
  return (
    <motion.div
      initial={reduce ? false : { opacity: 0, y: 18 }}
      animate={{ opacity: 1, y: 0 }}
      transition={{
        delay: 0.1 + step * 0.11,
        duration: 0.6,
        ease: [0.22, 1, 0.36, 1],
      }}
      {...props}
    >
      {children}
    </motion.div>
  );
}
