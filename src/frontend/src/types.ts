// ---------------------------------------------------------------------------
// TypeScript types that mirror the backend Request/Response records exactly.
//
// Naming convention matches the C# record names one-to-one.
// Field names use camelCase, which is what System.Text.Json produces by default.
//
// Rule: one type per backend endpoint, never shared between endpoints — even
// when two types have identical fields they remain separate so each endpoint
// can evolve independently.
// ---------------------------------------------------------------------------

// ── AuthorsController ────────────────────────────────────────────────────────

/** Mirrors AuthorsController.CreateAuthorRequest */
export interface CreateAuthorRequest {
  name: string;
  bio?: string;
}

/** Mirrors AuthorsController.UpdateAuthorRequest */
export interface UpdateAuthorRequest {
  name: string;
  bio?: string;
}

/** Mirrors AuthorsController.GetAllAuthorsBookEntry */
export interface GetAllAuthorsBookEntry {
  id: number;
  title: string;
  publishedYear: number;
}

/** Mirrors AuthorsController.GetAllAuthorsResponse */
export interface GetAllAuthorsResponse {
  id: number;
  name: string;
  bio?: string;
  books: GetAllAuthorsBookEntry[];
}

/** Mirrors AuthorsController.GetAuthorByIdBookEntry */
export interface GetAuthorByIdBookEntry {
  id: number;
  title: string;
  publishedYear: number;
}

/** Mirrors AuthorsController.GetAuthorByIdResponse */
export interface GetAuthorByIdResponse {
  id: number;
  name: string;
  bio?: string;
  books: GetAuthorByIdBookEntry[];
}

/** Mirrors AuthorsController.CreateAuthorResponse */
export interface CreateAuthorResponse {
  id: number;
  name: string;
  bio?: string;
}

// ── GenresController ─────────────────────────────────────────────────────────

/** Mirrors GenresController.CreateGenreRequest */
export interface CreateGenreRequest {
  name: string;
}

/** Mirrors GenresController.UpdateGenreRequest */
export interface UpdateGenreRequest {
  name: string;
}

/** Mirrors GenresController.GetAllGenresResponse */
export interface GetAllGenresResponse {
  id: number;
  name: string;
}

/** Mirrors GenresController.GetGenreByIdResponse */
export interface GetGenreByIdResponse {
  id: number;
  name: string;
}

/** Mirrors GenresController.CreateGenreResponse */
export interface CreateGenreResponse {
  id: number;
  name: string;
}

// ── BooksController ──────────────────────────────────────────────────────────

/** Mirrors BooksController.CreateBookRequest */
export interface CreateBookRequest {
  title: string;
  publishedYear: number;
  authorId: number;
}

/** Mirrors BooksController.UpdateBookRequest */
export interface UpdateBookRequest {
  title: string;
  publishedYear: number;
  authorId: number;
}

/** Mirrors BooksController.GetAllBooksAuthorEntry */
export interface GetAllBooksAuthorEntry {
  id: number;
  name: string;
}

/** Mirrors BooksController.GetAllBooksGenreEntry */
export interface GetAllBooksGenreEntry {
  id: number;
  name: string;
}

/** Mirrors BooksController.GetAllBooksResponse */
export interface GetAllBooksResponse {
  id: number;
  title: string;
  publishedYear: number;
  author?: GetAllBooksAuthorEntry;
  genres: GetAllBooksGenreEntry[];
}

/** Mirrors BooksController.GetBookByIdAuthorEntry */
export interface GetBookByIdAuthorEntry {
  id: number;
  name: string;
}

/** Mirrors BooksController.GetBookByIdGenreEntry */
export interface GetBookByIdGenreEntry {
  id: number;
  name: string;
}

/** Mirrors BooksController.GetBookByIdReviewEntry */
export interface GetBookByIdReviewEntry {
  id: number;
  reviewerName: string;
  rating: number;
  comment?: string;
}

/** Mirrors BooksController.GetBookByIdResponse */
export interface GetBookByIdResponse {
  id: number;
  title: string;
  publishedYear: number;
  author?: GetBookByIdAuthorEntry;
  genres: GetBookByIdGenreEntry[];
  reviews: GetBookByIdReviewEntry[];
}

/** Mirrors BooksController.CreateBookResponse */
export interface CreateBookResponse {
  id: number;
  title: string;
  publishedYear: number;
  authorId: number;
}

// ── ReviewsController ────────────────────────────────────────────────────────

/** Mirrors ReviewsController.CreateReviewRequest */
export interface CreateReviewRequest {
  reviewerName: string;
  rating: number;
  comment?: string;
}

/** Mirrors ReviewsController.UpdateReviewRequest */
export interface UpdateReviewRequest {
  reviewerName: string;
  rating: number;
  comment?: string;
}

/** Mirrors ReviewsController.GetAllReviewsResponse */
export interface GetAllReviewsResponse {
  id: number;
  bookId: number;
  reviewerName: string;
  rating: number;
  comment?: string;
}

/** Mirrors ReviewsController.GetReviewByIdResponse */
export interface GetReviewByIdResponse {
  id: number;
  bookId: number;
  reviewerName: string;
  rating: number;
  comment?: string;
}

/** Mirrors ReviewsController.CreateReviewResponse */
export interface CreateReviewResponse {
  id: number;
  bookId: number;
  reviewerName: string;
  rating: number;
  comment?: string;
}

// ── BookGenresController ─────────────────────────────────────────────────────

/** Mirrors BookGenresController.GetAllBookGenresResponse */
export interface GetAllBookGenresResponse {
  id: number;
  name: string;
}
