import { useId } from "react";

// Flags are inline SVG rather than emoji: Windows doesn't render flag emoji (it shows "SE"/"GB" instead).

export function SwedishFlag({ className }: { className?: string }) {
  return (
    <svg className={className} viewBox="0 0 16 10" aria-hidden="true" focusable="false">
      <rect width="16" height="10" fill="#006AA7" />
      <rect x="5" width="2" height="10" fill="#FECC00" />
      <rect y="4" width="16" height="2" fill="#FECC00" />
    </svg>
  );
}

export function BritishFlag({ className }: { className?: string }) {
  const id = useId();
  const clipFlag = `${id}-flag`;
  const clipDiagonals = `${id}-diagonals`;

  return (
    <svg className={className} viewBox="0 0 60 30" aria-hidden="true" focusable="false">
      <clipPath id={clipFlag}>
        <path d="M0,0 v30 h60 v-30 z" />
      </clipPath>
      <clipPath id={clipDiagonals}>
        <path d="M30,15 h30 v15 z v15 h-30 z h-30 v-15 z v-15 h30 z" />
      </clipPath>
      <g clipPath={`url(#${clipFlag})`}>
        <path d="M0,0 v30 h60 v-30 z" fill="#012169" />
        <path d="M0,0 L60,30 M60,0 L0,30" stroke="#fff" strokeWidth="6" />
        <path d="M0,0 L60,30 M60,0 L0,30" clipPath={`url(#${clipDiagonals})`} stroke="#C8102E" strokeWidth="4" />
        <path d="M30,0 v30 M0,15 h60" stroke="#fff" strokeWidth="10" />
        <path d="M30,0 v30 M0,15 h60" stroke="#C8102E" strokeWidth="6" />
      </g>
    </svg>
  );
}
