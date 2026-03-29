import { useEffect, useState } from "react";
import type { GetAllAuthorsResponse } from "../types";

// The base URL for the backend API.
// When running locally, the backend defaults to http://localhost:5000.
const API_BASE = "http://localhost:5000";

/**
 * AuthorList component
 *
 * Fetches all authors from the API and renders them as a simple list.
 * Demonstrates a basic fetch-on-mount pattern using useEffect + useState.
 */
export default function AuthorList() {
  const [authors, setAuthors] = useState<GetAllAuthorsResponse[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    fetch(`${API_BASE}/api/authors`)
      .then((res) => {
        if (!res.ok) throw new Error(`HTTP ${res.status}`);
        return res.json() as Promise<GetAllAuthorsResponse[]>;
      })
      .then(setAuthors)
      .catch((err: unknown) =>
        setError(err instanceof Error ? err.message : "Unknown error")
      )
      .finally(() => setLoading(false));
  }, []);

  if (loading) return <p>Loading authors…</p>;
  if (error) return <p role="alert">Error: {error}</p>;

  return (
    <section>
      <h2>Authors</h2>
      {authors.length === 0 ? (
        <p>No authors found.</p>
      ) : (
        <ul>
          {authors.map((author) => (
            <li key={author.id}>
              <strong>{author.name}</strong>
              {author.bio && <span> — {author.bio}</span>}
            </li>
          ))}
        </ul>
      )}
    </section>
  );
}
