import { useEffect } from "react";
import { AsyncContent } from "../../components/AsyncContent";
import { useProfile } from "./queries";

export function Hero() {
  const query = useProfile();
  const name = query.data?.name;

  useEffect(() => {
    if (name) document.title = `${name} · CV`;
  }, [name]);

  return (
    <header className="hero">
      <AsyncContent query={query}>
        {(profile) => (
          <>
            <h1>{profile.name}</h1>
            <p className="headline">{profile.headline}</p>
            <p className="summary">{profile.summary}</p>
            {profile.links.length > 0 && (
              <ul className="hero-links">
                {profile.links.map((link) => (
                  <li key={link.url}>
                    <a href={link.url} target="_blank" rel="noopener noreferrer">
                      {link.label}
                    </a>
                  </li>
                ))}
              </ul>
            )}
          </>
        )}
      </AsyncContent>
    </header>
  );
}
