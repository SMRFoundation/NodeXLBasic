/* =============================================================================
 * data.js  —  A small, HAND-AUTHORED "embedding space"
 * -----------------------------------------------------------------------------
 * Real RAG systems turn each word/document into a vector of a few hundred to a
 * few thousand numbers produced by a neural network. Those numbers are opaque.
 *
 * To *teach* what an embedding is, this file uses the same idea with numbers a
 * human can read. Every word is a vector over ~16 INTERPRETABLE semantic axes
 * (how "aquatic", how "technological", how "emotional", ...). Words that share
 * meaning share axes, so cosine similarity between these vectors behaves like a
 * real embedding: neighbours are semantically related, and a "bridge" word
 * (e.g. SUBMARINE) scores high on two different themes at once.
 *
 * The board you see is a 2-D PROJECTION (PCA) of this 16-D space — exactly the
 * move a data scientist makes with t-SNE/UMAP to look at real embeddings.
 * ===========================================================================*/

(function (global) {
  "use strict";

  // The interpretable axes of our toy embedding model.
  const DIMS = [
    "water",     // aquatic / liquid
    "tech",      // machinery / computation
    "affect",    // emotional charge
    "nature",    // organic / wild / living
    "cosmos",    // astronomical / vast
    "sound",     // auditory / musical
    "food",      // edible / culinary
    "money",     // economic / value
    "motion",    // movement / dynamism
    "abstract",  // conceptual vs. concrete
    "social",    // people / interpersonal
    "structure", // order / system / built
    "light",     // luminous / visual
    "scale",     // bigness / depth / magnitude
    "warmth",    // temperature / comfort
    "danger",    // risk / threat
  ];

  // Themed clusters ("concept regions" / the tiles of the game board).
  const CLUSTERS = {
    ocean:  { name: "Ocean",      color: "#2f9bd6", blurb: "seas, tides, deep water" },
    tech:   { name: "Technology", color: "#17c0b8", blurb: "computing & machines" },
    affect: { name: "Emotion",    color: "#e0518e", blurb: "feelings & moods" },
    nature: { name: "Nature",     color: "#54b04a", blurb: "forests & wildlife" },
    cosmos: { name: "Cosmos",     color: "#7b6ef6", blurb: "stars & space" },
    sound:  { name: "Music",      color: "#f0932b", blurb: "sound & melody" },
    food:   { name: "Food",       color: "#c98a3b", blurb: "cooking & flavor" },
    money:  { name: "Finance",    color: "#c9b537", blurb: "markets & value" },
  };

  // Compact authoring helper: w(word, cluster, {dim:value, ...})
  // Values are 0..1 "how much of this axis". Vectors are normalized in engine.js.
  const raw = [];
  const w = (word, cluster, vec) => raw.push({ word, cluster, vec });

  /* ---- OCEAN ------------------------------------------------------------- */
  w("wave",      "ocean", { water:.95, motion:.7, scale:.4 });
  w("tide",      "ocean", { water:.9, motion:.6, cosmos:.35, scale:.5 });      // moon-driven → cosmos
  w("coral",     "ocean", { water:.85, nature:.7, light:.4 });
  w("whale",     "ocean", { water:.85, nature:.8, scale:.85 });
  w("shark",     "ocean", { water:.85, nature:.7, danger:.75 });
  w("current",   "ocean", { water:.85, motion:.75, tech:.15 });
  w("harbor",    "ocean", { water:.8, structure:.6, social:.4 });
  w("sailor",    "ocean", { water:.75, social:.6, motion:.4 });
  w("shipwreck", "ocean", { water:.8, danger:.7, scale:.5, structure:.35 });
  w("plankton",  "ocean", { water:.85, nature:.75, scale:.15, food:.3 });
  w("reef",      "ocean", { water:.85, nature:.6, structure:.4 });
  w("saltwater", "ocean", { water:.97, food:.2 });
  w("abyss",     "ocean", { water:.8, scale:.9, danger:.5, abstract:.4 });
  w("lighthouse","ocean", { water:.6, light:.85, structure:.6, danger:.4 });
  w("seaweed",   "ocean", { water:.85, nature:.7, food:.4 });
  w("dolphin",   "ocean", { water:.85, nature:.8, social:.5, sound:.4 });

  /* ---- TECHNOLOGY -------------------------------------------------------- */
  w("computer",  "tech", { tech:.95, structure:.6, abstract:.4 });
  w("algorithm", "tech", { tech:.9, abstract:.8, structure:.7 });
  w("network",   "tech", { tech:.85, structure:.75, social:.4, abstract:.5 });
  w("server",    "tech", { tech:.9, structure:.7, scale:.3 });
  w("code",      "tech", { tech:.9, abstract:.7, structure:.65 });
  w("robot",     "tech", { tech:.9, motion:.6, social:.3 });
  w("circuit",   "tech", { tech:.9, structure:.7, light:.35 });
  w("software",  "tech", { tech:.9, abstract:.7, structure:.6 });
  w("internet",  "tech", { tech:.85, social:.6, structure:.6, abstract:.5, scale:.6 });
  w("database",  "tech", { tech:.9, structure:.8, abstract:.55 });
  w("sensor",    "tech", { tech:.85, light:.3, structure:.5 });
  w("laser",     "tech", { tech:.8, light:.9, danger:.4, motion:.4 });
  w("battery",   "tech", { tech:.8, structure:.5, warmth:.3 });
  w("processor", "tech", { tech:.92, structure:.7, warmth:.35 });
  w("pixel",     "tech", { tech:.8, light:.7, structure:.5, scale:.1 });

  /* ---- EMOTION ----------------------------------------------------------- */
  w("joy",       "affect", { affect:.95, warmth:.6, light:.5, abstract:.6 });
  w("fear",      "affect", { affect:.9, danger:.75, abstract:.6 });
  w("anger",     "affect", { affect:.9, warmth:.6, danger:.6 });
  w("love",      "affect", { affect:.95, warmth:.75, social:.7, abstract:.6 });
  w("grief",     "affect", { affect:.9, abstract:.6, social:.4 });
  w("hope",      "affect", { affect:.85, light:.6, abstract:.7 });
  w("anxiety",   "affect", { affect:.9, danger:.5, motion:.3, abstract:.6 });
  w("calm",      "affect", { affect:.75, warmth:.4, abstract:.55, motion:.1 });
  w("delight",   "affect", { affect:.9, warmth:.55, light:.5 });
  w("sorrow",    "affect", { affect:.9, abstract:.6, water:.2 });                // "tears"
  w("envy",      "affect", { affect:.85, social:.5, money:.35 });
  w("pride",     "affect", { affect:.85, social:.55, abstract:.55 });
  w("nostalgia", "affect", { affect:.85, abstract:.7, warmth:.4, sound:.2 });
  w("courage",   "affect", { affect:.8, danger:.4, abstract:.6, social:.3 });

  /* ---- NATURE ------------------------------------------------------------ */
  w("forest",    "nature", { nature:.95, scale:.6, structure:.3 });
  w("tree",      "nature", { nature:.9, scale:.5, structure:.3 });
  w("leaf",      "nature", { nature:.85, light:.4, scale:.15 });
  w("moss",      "nature", { nature:.85, water:.35, warmth:.2 });
  w("deer",      "nature", { nature:.9, motion:.5, social:.3 });
  w("mountain",  "nature", { nature:.8, scale:.9, structure:.3 });
  w("trail",     "nature", { nature:.7, motion:.55, structure:.4 });
  w("meadow",    "nature", { nature:.9, light:.5, warmth:.4 });
  w("wildlife",  "nature", { nature:.9, motion:.4, social:.2, scale:.4 });
  w("river",     "nature", { nature:.7, water:.85, motion:.7 });                 // NATURE↔OCEAN bridge
  w("owl",       "nature", { nature:.85, sound:.35, danger:.25 });
  w("honey",     "nature", { nature:.7, food:.75, warmth:.4 });                  // NATURE↔FOOD bridge
  w("garden",    "nature", { nature:.8, food:.45, structure:.4, social:.3 });

  /* ---- COSMOS ------------------------------------------------------------ */
  w("star",      "cosmos", { cosmos:.9, light:.85, scale:.7 });
  w("planet",    "cosmos", { cosmos:.9, scale:.8, structure:.3 });
  w("galaxy",    "cosmos", { cosmos:.95, scale:.95, light:.6 });
  w("orbit",     "cosmos", { cosmos:.85, motion:.7, structure:.5, abstract:.4 });
  w("comet",     "cosmos", { cosmos:.85, motion:.75, light:.5, danger:.3 });
  w("nebula",    "cosmos", { cosmos:.9, light:.7, scale:.85, abstract:.3 });
  w("gravity",   "cosmos", { cosmos:.7, abstract:.7, motion:.5, scale:.6 });
  w("cosmos",    "cosmos", { cosmos:.95, scale:.95, abstract:.5 });
  w("eclipse",   "cosmos", { cosmos:.85, light:.6, danger:.3, motion:.3 });
  w("meteor",    "cosmos", { cosmos:.8, motion:.8, danger:.6, light:.5 });
  w("moon",      "cosmos", { cosmos:.85, light:.6, water:.25 });                 // COSMOS↔OCEAN (tides)
  w("telescope", "cosmos", { cosmos:.7, tech:.75, light:.5, structure:.5 });     // COSMOS↔TECH bridge

  /* ---- MUSIC ------------------------------------------------------------- */
  w("melody",    "sound", { sound:.95, affect:.5, abstract:.5 });
  w("rhythm",    "sound", { sound:.9, motion:.6, structure:.4 });
  w("guitar",    "sound", { sound:.9, structure:.4, social:.3 });
  w("symphony",  "sound", { sound:.95, scale:.6, structure:.6, social:.4 });
  w("chord",     "sound", { sound:.9, structure:.6, abstract:.4 });
  w("drum",      "sound", { sound:.9, motion:.55, warmth:.3 });
  w("harmony",   "sound", { sound:.85, structure:.5, affect:.4, social:.4 });
  w("singer",    "sound", { sound:.85, social:.6, affect:.5 });
  w("echo",      "sound", { sound:.85, motion:.4, scale:.5, abstract:.3 });
  w("lullaby",   "sound", { sound:.85, affect:.6, warmth:.5, social:.4 });       // MUSIC↔EMOTION bridge
  w("ballad",    "sound", { sound:.85, affect:.65, social:.4, abstract:.4 });    // MUSIC↔EMOTION bridge
  w("tempo",     "sound", { sound:.8, motion:.6, structure:.5, abstract:.4 });

  /* ---- FOOD -------------------------------------------------------------- */
  w("bread",     "food", { food:.9, warmth:.5, structure:.3 });
  w("spice",     "food", { food:.9, warmth:.6, money:.3 });                      // FOOD↔FINANCE (spice trade)
  w("kitchen",   "food", { food:.8, structure:.6, warmth:.5, social:.4 });
  w("recipe",    "food", { food:.85, structure:.6, abstract:.5 });
  w("flavor",    "food", { food:.9, affect:.35, abstract:.4 });
  w("roast",     "food", { food:.85, warmth:.75, motion:.2 });
  w("chef",      "food", { food:.8, social:.55, structure:.4 });
  w("oven",      "food", { food:.75, warmth:.8, tech:.35, structure:.5 });       // FOOD↔TECH (appliance)
  w("harvest",   "food", { food:.75, nature:.7, money:.35, social:.4 });         // FOOD↔NATURE↔FINANCE
  w("dessert",   "food", { food:.9, warmth:.4, affect:.4 });
  w("broth",     "food", { food:.85, water:.55, warmth:.6 });                    // FOOD↔OCEAN (liquid)
  w("feast",     "food", { food:.85, social:.65, affect:.5, warmth:.4 });

  /* ---- FINANCE ----------------------------------------------------------- */
  w("market",    "money", { money:.9, social:.55, structure:.5 });
  w("profit",    "money", { money:.95, abstract:.5 });
  w("bank",      "money", { money:.9, structure:.7, social:.4 });
  w("invest",    "money", { money:.9, abstract:.6, motion:.3 });
  w("currency",  "money", { money:.95, abstract:.5, structure:.4 });
  w("trade",     "money", { money:.85, social:.55, motion:.4 });
  w("stock",     "money", { money:.9, abstract:.55, motion:.4, structure:.4 });
  w("wealth",    "money", { money:.95, scale:.5, social:.4 });
  w("debt",      "money", { money:.9, affect:.35, danger:.4 });                  // FINANCE↔EMOTION (stress)
  w("coin",      "money", { money:.85, light:.4, structure:.3 });
  w("auction",   "money", { money:.85, social:.6, sound:.3 });
  w("greed",     "money", { money:.7, affect:.75, danger:.4 });                  // FINANCE↔EMOTION bridge

  /* ---- BRIDGE WORDS (deliberately span two themes) ----------------------- */
  w("submarine", "ocean",  { water:.8, tech:.8, scale:.5, danger:.35 });         // OCEAN↔TECH
  w("sonar",     "tech",   { tech:.8, water:.7, sound:.55 });                    // OCEAN↔TECH↔MUSIC
  w("cable",     "tech",   { tech:.75, water:.5, structure:.6, scale:.4 });      // OCEAN↔TECH (undersea)
  w("radar",     "tech",   { tech:.8, cosmos:.4, motion:.4, danger:.3 });        // TECH↔COSMOS
  w("satellite", "tech",   { tech:.8, cosmos:.75, structure:.5, motion:.4 });    // TECH↔COSMOS bridge
  w("navigation","ocean",  { water:.6, cosmos:.5, tech:.5, motion:.5 });         // OCEAN↔COSMOS↔TECH
  w("bitcoin",   "money",  { money:.8, tech:.8, abstract:.6 });                  // FINANCE↔TECH bridge
  w("blockchain","tech",   { tech:.85, money:.65, structure:.7, abstract:.6 });  // TECH↔FINANCE
  w("mining",    "money",  { money:.6, tech:.55, nature:.4, danger:.35 });       // FINANCE↔TECH↔NATURE
  w("melancholy","affect", { affect:.85, sound:.55, abstract:.6, water:.2 });    // EMOTION↔MUSIC
  w("mood",      "affect", { affect:.8, sound:.4, abstract:.55 });               // EMOTION↔MUSIC
  w("otter",     "ocean",  { water:.7, nature:.8, social:.4 });                  // OCEAN↔NATURE bridge
  w("coast",     "ocean",  { water:.75, nature:.55, structure:.35 });            // OCEAN↔NATURE
  w("herb",      "food",   { food:.7, nature:.75, warmth:.3 });                  // FOOD↔NATURE bridge
  w("commerce",  "money",  { money:.85, social:.6, structure:.5, motion:.3 });   // FINANCE↔SOCIAL
  w("stardust",  "cosmos", { cosmos:.8, light:.6, nature:.35, abstract:.4 });    // COSMOS↔NATURE
  w("volcano",   "nature", { nature:.7, warmth:.85, danger:.75, scale:.6 });     // NATURE↔DANGER↔WARMTH
  w("storm",     "ocean",  { water:.6, motion:.8, danger:.7, sound:.4, scale:.5 });// OCEAN↔MOTION↔DANGER
  w("heartbeat", "affect", { affect:.6, sound:.6, motion:.6, nature:.3 });       // EMOTION↔MUSIC↔MOTION

  // Attach an index to each entry for stable identity.
  const WORDS = raw.map((r, i) => ({ id: i, ...r }));

  global.RAG_DATA = { DIMS, CLUSTERS, WORDS };
})(typeof window !== "undefined" ? window : globalThis);
