import { useState } from "react";
import AuthorList from "./components/AuthorList";
import BookDetail from "./components/BookDetail";
import BookList from "./components/BookList";
import "./App.css";

/**
 * App — root component
 *
 * Manages a simple "page" state:
 *  - "list"   → shows the BookList and AuthorList side by side
 *  - "detail" → shows the BookDetail for the selected book
 */
function App() {
  // null means "show the list"; a number means "show that book's detail"
  const [selectedBookId, setSelectedBookId] = useState<number | null>(null);

  return (
    <main style={{ padding: "1rem", fontFamily: "sans-serif" }}>
      <h1>📚 BookStore</h1>

      {selectedBookId === null ? (
        // ── List view: books on the left, authors on the right ───────────
        <div style={{ display: "flex", gap: "2rem" }}>
          {/* BookList receives a callback to navigate to the detail view */}
          <BookList onSelect={(id) => setSelectedBookId(id)} />
          <AuthorList />
        </div>
      ) : (
        // ── Detail view: single book with reviews ─────────────────────────
        <BookDetail
          bookId={selectedBookId}
          onBack={() => setSelectedBookId(null)}
        />
      )}
    </main>
  );
}

export default App;
